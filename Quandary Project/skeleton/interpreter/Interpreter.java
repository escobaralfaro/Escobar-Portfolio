package interpreter;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.atomic.AtomicReference;

import java.io.*;
import java.util.HashMap;
import java.util.Random;

import parser.ParserWrapper;
import ast.*;

public class Interpreter {

    // Process return codes
    public static final int EXIT_SUCCESS = 0;
    public static final int EXIT_PARSING_ERROR = 1;
    public static final int EXIT_STATIC_CHECKING_ERROR = 2;
    public static final int EXIT_DYNAMIC_TYPE_ERROR = 3;
    public static final int EXIT_NIL_REF_ERROR = 4;
    public static final int EXIT_QUANDARY_HEAP_OUT_OF_MEMORY_ERROR = 5;
    public static final int EXIT_DATA_RACE_ERROR = 6;
    public static final int EXIT_NONDETERMINISM_ERROR = 7;

    static private Interpreter interpreter;

    public static Interpreter getInterpreter() {
        return interpreter;
    }

    public static void main(String[] args) {
        String gcType = "NoGC"; // default for skeleton, which only supports NoGC
        long heapBytes = 1 << 14;
        int i = 0;
        String filename;
        long quandaryArg;
        try {
            for (; i < args.length; i++) {
                String arg = args[i];
                if (arg.startsWith("-")) {
                    if (arg.equals("-gc")) {
                        gcType = args[i + 1];
                        i++;
                    } else if (arg.equals("-heapsize")) {
                        heapBytes = Long.valueOf(args[i + 1]);
                        i++;
                    } else {
                        throw new RuntimeException("Unexpected option " + arg);
                    }
                } else {
                    if (i != args.length - 2) {
                        throw new RuntimeException("Unexpected number of arguments");
                    }
                    break;
                }
            }
            filename = args[i];
            quandaryArg = Long.valueOf(args[i + 1]);
        } catch (Exception ex) {
            System.out.println("Expected format: quandary [OPTIONS] QUANDARY_PROGRAM_FILE INTEGER_ARGUMENT");
            System.out.println("Options:");
            System.out.println("  -gc (MarkSweep|Explicit|NoGC)");
            System.out.println("  -heapsize BYTES");
            System.out.println("BYTES must be a multiple of the word size (8)");
            return;
        }

        Program astRoot = null;
        Reader reader;
        try {
            reader = new BufferedReader(new FileReader(filename));
        } catch (IOException ex) {
            throw new RuntimeException(ex);
        }
        try {
            astRoot = ParserWrapper.parse(reader);
        } catch (Exception ex) {
            ex.printStackTrace();
            Interpreter.fatalError("Uncaught parsing error: " + ex, Interpreter.EXIT_PARSING_ERROR);
        }
        //uncommented for debugging
        
    //    astRoot.println(System.out);
        interpreter = new Interpreter(astRoot);
        interpreter.initMemoryManager(gcType, heapBytes);
        String returnValueAsString = interpreter.executeRoot(astRoot, quandaryArg).toString(); // error here looks like
        System.out.println("Interpreter returned " + returnValueAsString);
        
    }

    final Program astRoot;
    final Random random;

    private Interpreter(Program astRoot) {
        this.astRoot = astRoot;
        this.random = new Random();
    }

    void initMemoryManager(String gcType, long heapBytes) {
        if (gcType.equals("Explicit")) {
            throw new RuntimeException("Explicit not implemented");            
        } else if (gcType.equals("MarkSweep")) {
            throw new RuntimeException("MarkSweep not implemented");            
        } else if (gcType.equals("RefCount")) {
            throw new RuntimeException("RefCount not implemented");            
        } else if (gcType.equals("NoGC")) {
            // Nothing to do
        }
    }

    class MyThread implements Runnable {
        private final int threadID;
        private final Expr expr;
        private final HashMap<String, QVal> symbol;
        private final HashMap<String, FuncDef> func;
        private QVal result;
    
        public MyThread(int threadID, Expr expr, HashMap<String, QVal> symbol, HashMap<String, FuncDef> func) {
            this.threadID = threadID;
            this.expr = expr;
            this.symbol = symbol;
            this.func = func;
        }
    
        public void run() {
            result = evaluate(expr, symbol, func);
        }
    
        public int getThreadID() {
            return threadID;
        }
    
        public QVal getResult() {
            return result;
        }
    }

    Object executeRoot(Program astRoot, long arg) {
        HashMap<String, QVal> symbol = new HashMap<String, QVal>();
        HashMap<String, FuncDef> funcDefMap = new HashMap<String, FuncDef>();
        FuncDefList funcList = astRoot.getFuncDefList();
        buildFuncDefMap(funcList, funcDefMap);
        FuncDef main = funcDefMap.get("main");
        QIntVal QArg = new QIntVal(arg);
        QArg.mutable = main.getFuncArg().getVarDecl().isMutable();
        symbol.put(main.getFuncArg().getVarDecl().getName(),QArg );
        return evaluateFunction(main, symbol, funcDefMap);
    }
    
    void buildFuncDefMap(FuncDefList funcList, HashMap<String, FuncDef> funcDefMap){
        FuncDef temp;
        FuncDefList list = funcList;
        while(list != null ){
            temp = list.getFunc();
            String funcArgIdent =  temp.getFuncId().getName();
            funcDefMap.put(funcArgIdent, temp);
            list = list.getFuncList();
        }
    }

    QVal evaluateFunction(FuncDef function, HashMap<String, QVal> symbol, HashMap<String, FuncDef> funcDefMap){
        StmtList stmtList = function.getStmtList();
        QVal retu = executeStmtList(stmtList, symbol, funcDefMap);
       // System.out.println("Running func:" + func);
        return retu;
    }

    QVal executeStmt(Stmt stmt, HashMap<String, QVal> symbol, HashMap<String, FuncDef> funcs){
        if (stmt instanceof VarDeclStmt){
            VarDeclStmt varstmt = (VarDeclStmt)stmt;
           // System.out.println("varstmt expr: "+varstmt.getExpr());
            QVal value = evaluate(varstmt.getExpr(), symbol, funcs);
            VarDecl vardecl = varstmt.getVarDecl();
            String varName = vardecl.getName();
            value.mutable = vardecl.isMutable();
            symbol.put(varName, value); //check if it already exisits
            return null;
        } else if(stmt instanceof AssignStmt) {
            AssignStmt a = (AssignStmt) stmt;
            String id = a.getIdent();
            Expr e = a.getExpr();
            QVal val = evaluate(e, symbol, funcs);
            if(symbol.containsKey(id)){
                QVal prevVal = symbol.remove(id);
                val.mutable = prevVal.mutable;
                symbol.put(id, val);
            } else {
                throw new RuntimeException("Variable not declared");
            }
            return null;
        } else if(stmt instanceof CallStmt){
            CallStmt cStmt = (CallStmt) stmt;
            String id = cStmt.getIdent();
            ExprList elist = cStmt.getExprList();
            if(id.equals("randomInt")){
                CallExpr.randomInt(( (QIntVal) evaluate(elist.getExpr(), symbol, funcs)).getInt());
            } else if(id.equals("left")){
                CallStmt.left(((QRefVal) evaluate(elist.getExpr(), symbol, funcs)));
            } else if(id.equals("right")){
                CallStmt.right(((QRefVal) evaluate(elist.getExpr(), symbol, funcs)));
            } else if(id.equals("isAtom")){
                CallStmt.isAtom(evaluate(elist.getExpr(), symbol, funcs));
            } else if(id.equals("isNil")){
                CallStmt.isNil(evaluate(elist.getExpr(), symbol, funcs));
            } else if(id.equals("setLeft")){
                CallStmt.setLeft((QRefVal)evaluate(elist.getExpr(), symbol, funcs), evaluate(elist.getExprList().getExpr(), symbol, funcs));
            } else if(id.equals("setRight")){
                CallStmt.setRight((QRefVal)evaluate(elist.getExpr(), symbol, funcs), evaluate(elist.getExprList().getExpr(), symbol, funcs));               
            } else if(id.equals("acq")) {
                CallStmt.acq((QRefVal)evaluate(elist.getExpr(), symbol, funcs));   
            } else if(id.equals("rel")) {
                CallStmt.rel((QRefVal)evaluate(elist.getExpr(), symbol, funcs));
            }else {
                FuncDef f = funcs.get(id);
                HashMap<String, QVal> innerScope = new HashMap<String, QVal>();
                executeExprList(f.getFuncArg(), elist, innerScope, symbol, funcs);
                evaluateFunction(f, innerScope, funcs);
            }
            return null;

        }else if (stmt instanceof IfStmt) {
            IfStmt i = (IfStmt)stmt;
            Cond cond = i.getCondition();
            Stmt sBlock = i.getStmt();
            if(evaluateCond(cond, symbol, funcs)){
                return executeStmt(sBlock, symbol, funcs);
            }
            return null;
        } else if (stmt instanceof IfElseStmt){
            IfElseStmt i = (IfElseStmt)stmt;
            Cond cond = i.getCondition();
            Stmt sIfBlock = i.getLeftStmt();
            Stmt sElseBlock = i.getRightStmt();
            if(evaluateCond(cond, symbol, funcs)){
                return executeStmt(sIfBlock, symbol, funcs);
            } else {
                return executeStmt(sElseBlock, symbol, funcs);
            } 
        } else if (stmt instanceof WhileStmt){
            WhileStmt w = (WhileStmt)stmt;
            Cond cond = w.getCond();
            Stmt whilest = w.getStmt();
            QVal ret = null;
            boolean ans;
            while(ans = evaluateCond(cond, symbol, funcs)){

                System.out.println("stmt:" + stmt);
                System.out.println("cond:" + ans);
                System.out.println("conddd:" + cond);

                ret = executeStmt(whilest, symbol, funcs); // check here if issue
                if (ret != null) break;
            }
            return ret;
        } else if( stmt instanceof ReturnStmt){
            ReturnStmt retStmt = (ReturnStmt) stmt;
            return evaluate(retStmt.getExpr(), symbol, funcs);
        } else if (stmt instanceof StmtList){
            return executeStmtList((StmtList)stmt, symbol, funcs);
        }else if(stmt instanceof PrintStmt){
            PrintStmt print = (PrintStmt) stmt;
            String str = evaluate(print.getExpr(), symbol, funcs).toString();
            System.out.println(str);
            return null;
        } else {
            throw new RuntimeException("Unhandled Stmt type" + stmt);
        }

    }

    QVal executeStmtList(StmtList stmtlist, HashMap<String, QVal> symbol, HashMap<String, FuncDef> funcs){
        QVal ret ;
        HashMap<String, QVal> copiedMap = (HashMap<String, QVal>) symbol.clone();
        while (stmtlist != null){
            Stmt s = stmtlist.getStmt();
            stmtlist = stmtlist.getStmtList();
            ret = executeStmt(s, copiedMap, funcs);
            if(ret != null){
                return ret;
            }
            updateMap(symbol, copiedMap);
        }
        return null;
    }

    void updateMap(HashMap<String, QVal> oldMap, HashMap<String, QVal> copiedMap) {
        for (HashMap.Entry<String, QVal> entry : copiedMap.entrySet()) {
            String key = entry.getKey();
            QVal copiedValue = entry.getValue();
    
            // Check if the key exists in the original map
            if (oldMap.containsKey(key)) {
                // Update the value in the original map
                oldMap.remove(key);
                oldMap.put(key, copiedValue);
            }
        }
    }



    Boolean evaluateCond(Cond condition, HashMap<String, QVal> symbol, HashMap<String, FuncDef> funcs){
        if (condition instanceof CondRelation){
            CondRelation c = (CondRelation) condition;
            Expr leftSide = c.getLeftExpr();
            Expr rightSide = c.getRightExpr();
            int comparator = c.getOperator();
            switch(comparator) {
                case CondRelation.LESSTH: return ((QIntVal)evaluate(leftSide, symbol, funcs)).getInt() < ((QIntVal)evaluate(rightSide, symbol, funcs)).getInt();
                case CondRelation.GREATERTH: return ((QIntVal)evaluate(leftSide, symbol, funcs)).getInt() > ((QIntVal)evaluate(rightSide, symbol, funcs)).getInt();
                case CondRelation.LESSEQ: return ((QIntVal)evaluate(leftSide, symbol, funcs)).getInt() <= ((QIntVal)evaluate(rightSide, symbol, funcs)).getInt();
                case CondRelation.EQ: return ((QIntVal)evaluate(leftSide, symbol, funcs)).getInt() == (((QIntVal)evaluate(rightSide, symbol, funcs)).getInt());
                case CondRelation.GREATEREQ: return ((QIntVal)evaluate(leftSide, symbol, funcs)).getInt() >= ((QIntVal)evaluate(rightSide, symbol, funcs)).getInt();
                case CondRelation.NOTEQ: return ((QIntVal)evaluate(leftSide, symbol, funcs)).getInt() != ((QIntVal)evaluate(rightSide, symbol, funcs)).getInt();
            } 
            
        } else if(condition instanceof LogicCondition){
            LogicCondition c = (LogicCondition) condition;
            Cond leftSide = c.getLeftCond();
            Cond rightSide = c.getRightCond();
            //int comparator = c.getOperator();
            switch(c.getOperator()) {
                case LogicCondition.AND: return evaluateCond(leftSide, symbol, funcs) && evaluateCond(rightSide, symbol, funcs);
                case LogicCondition.NOT: return !evaluateCond(leftSide, symbol, funcs);
                case LogicCondition.OR: return evaluateCond(leftSide, symbol, funcs) || evaluateCond(rightSide, symbol, funcs);
            } 

        } 
            throw new RuntimeException("Unexpected Cond Type");
        
    }

    QVal evaluate(Expr expr, HashMap<String, QVal> symbol, HashMap<String, FuncDef> funcs) {
        if (expr instanceof ConstExpr) {
            return ((ConstExpr)expr).getValue();
        } else if(expr instanceof VarExpr ) {
            VarExpr v = (VarExpr) expr;
            String vname = v.getVarName();
            if(symbol.containsKey(vname)){
                return symbol.get(vname);
            }
        } else if ( expr instanceof UnaryExpr) {
            UnaryExpr uexpr = (UnaryExpr)expr; 
                return new QIntVal(0 - ((QIntVal)evaluate(uexpr.getExpr(), symbol, funcs)).getInt());
        } else if (expr instanceof BinaryExpr) {
            BinaryExpr binaryExpr = (BinaryExpr)expr;
            if (binaryExpr.isConcurrent()) {
                MyThread leftThread = new MyThread(1, binaryExpr.getLeftExpr(), symbol, funcs); // Assign ID 1 to left thread
                MyThread rightThread = new MyThread(2, binaryExpr.getRightExpr(), symbol, funcs); // Assign ID 2 to right thread
                Thread t1 = new Thread(leftThread);
                Thread t2 = new Thread(rightThread);                
                t1.start();
                t2.start();

                try {
                    t1.join();
                    t2.join();
                } catch (InterruptedException e) {
                    throw new RuntimeException("Thread execution interrupted", e);
                }  
                QVal leftValue = leftThread.getResult();
                QVal rightValue = rightThread.getResult();
                switch (binaryExpr.getOperator()){
                    case BinaryExpr.PLUS: return new QIntVal( ((QIntVal)leftValue).getInt() + ((QIntVal)rightValue).getInt() );
                    case BinaryExpr.MINUS: return new QIntVal( ((QIntVal)leftValue).getInt() - ((QIntVal)rightValue).getInt() );
                    case BinaryExpr.TIMES: return new QIntVal( ((QIntVal)leftValue).getInt() * ((QIntVal)rightValue).getInt() );
                    case BinaryExpr.DOT: return new QRefVal(false, new QObj(leftValue, rightValue));
                    default: throw new RuntimeException("Unhandled operator during concurrency");
                }
            } else {
                switch (binaryExpr.getOperator()) {
                    case BinaryExpr.PLUS: return new QIntVal(((QIntVal)evaluate(binaryExpr.getLeftExpr(), symbol, funcs)).getInt() + ((QIntVal)evaluate(binaryExpr.getRightExpr(), symbol, funcs)).getInt());
                    case BinaryExpr.MINUS: return new QIntVal(((QIntVal)evaluate(binaryExpr.getLeftExpr(), symbol, funcs)).getInt() - ((QIntVal)evaluate(binaryExpr.getRightExpr(), symbol, funcs)).getInt());
                    case BinaryExpr.TIMES: return new QIntVal(((QIntVal)evaluate(binaryExpr.getLeftExpr(), symbol, funcs)).getInt() * ((QIntVal)evaluate(binaryExpr.getRightExpr(), symbol, funcs)).getInt());
                    case BinaryExpr.DOT : return new QRefVal(false, new QObj(evaluate(binaryExpr.getLeftExpr(), symbol, funcs), evaluate(binaryExpr.getRightExpr(), symbol, funcs)));
                    default: throw new RuntimeException("Unhandled operator");
                    }
            }
        } else if (expr instanceof CallExpr){
            CallExpr callexpr = (CallExpr)expr;
            String ident = callexpr.getIdent();
            ExprList elist = callexpr.getExprList();
            if(ident.equals("randomInt")){
                long intValue = ((QIntVal) evaluate(elist.getExpr(), symbol, funcs)).getInt();
                return new QIntVal(CallExpr.randomInt(intValue));
            } else if(ident.equals("left")){
                return CallStmt.left(((QRefVal) evaluate(elist.getExpr(), symbol, funcs)));
            } else if(ident.equals("right")){
                return CallStmt.right(((QRefVal) evaluate(elist.getExpr(), symbol, funcs)));
            } else if(ident.equals("isAtom")){
                return CallStmt.isAtom(evaluate(elist.getExpr(), symbol, funcs));
            } else if(ident.equals("isNil")){
                return CallStmt.isNil(evaluate(elist.getExpr(), symbol, funcs));
            } else if(ident.equals("setLeft")){
                return CallStmt.setLeft((QRefVal)evaluate(elist.getExpr(), symbol, funcs), evaluate(elist.getExprList().getExpr(), symbol, funcs));
            } else if(ident.equals("setRight")){
                return CallStmt.setRight((QRefVal)evaluate(elist.getExpr(), symbol, funcs), evaluate(elist.getExprList().getExpr(), symbol, funcs));               
            } else if (ident.equals("acq")){
                return CallStmt.acq((QRefVal)evaluate(elist.getExpr(), symbol, funcs));               
            } else if (ident.equals("rel")){
                return CallStmt.rel((QRefVal)evaluate(elist.getExpr(), symbol, funcs));               
            } 
            if(funcs.containsKey(ident)){
                FuncDef func = funcs.get(ident);
                HashMap<String, QVal> newScope = new HashMap<String, QVal>();
                //System.out.println("expr: " + callexpr);
                //System.out.println("elist: " + elist);
                executeExprList(func.getFuncArg(), elist, newScope, symbol, funcs);
                return evaluateFunction(func, newScope, funcs);
            } else {
                throw new RuntimeException("Unexpected Function call");
            }
        }else if (expr instanceof CastExpr){
            CastExpr c = (CastExpr)expr;
            Expr expr1 = c.getExpr();
            Type type = c.getType();
            QVal val = evaluate(expr1, symbol, funcs);
            switch(type.getVarType()){
                case Type.INT:
                if(val instanceof QIntVal) return val;
                return (QIntVal)evaluate(expr1, symbol, funcs);
                
                case Type.REF:
                if(val instanceof QRefVal) return val;
                return (QRefVal)evaluate(expr1, symbol, funcs);

                case Type.Q:
                if(val instanceof QVal) return val;
                return (QVal)evaluate(expr1, symbol, funcs);

                default: throw new RuntimeException("Cast of undefinded type");
            }
        } 
            System.out.println("Expr:" + expr);
            throw new RuntimeException("Unhandled Expr type");
    }

    QVal executeExprList(FormalDeclList formalDeclList, ExprList elist, HashMap<String, QVal> innerContext, HashMap<String, QVal> context, HashMap<String, FuncDef> funcs){
        if(elist != null){
            ExprList neExprList = elist;
           // System.out.println("ExprList: " + elist);
            FormalDeclList fdl = formalDeclList;
            if(fdl !=null){
                //System.out.println(">1 parameter");
                FormalDeclList neFdl = fdl;
                //System.out.println("neFdl: " + neFdl);
               // System.out.println("neExprList: " + neExprList);
                while(neExprList != null){
                    VarDecl var = neFdl.getVarDecl();
                    neFdl = neFdl.getFuncParams();
                    Expr e = neExprList.getExpr();
                    neExprList = neExprList.getExprList();
                    QVal tempCheck = evaluate(e, context, funcs);
                    tempCheck.mutable = var.isMutable();
                    innerContext.put(var.getName(), tempCheck);
                }
            }
        }
        //System.out.println("innerContext: " + innerContext);
        return null;
    }

	public static void fatalError(String message, int processReturnCode) {
        System.out.println(message);
        System.exit(processReturnCode);
	}
}