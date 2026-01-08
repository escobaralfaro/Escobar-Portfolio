package ast;

public class FuncDef extends ASTNode {
    final VarDecl funcId;
    final FormalDeclList funcArg;
    final StmtList stmtList;

    public FuncDef(VarDecl funcId, FormalDeclList funcArg, StmtList stmtList, Location loc) {
        super(loc);
        this.funcId = funcId;
        this.funcArg = funcArg;
        this.stmtList = stmtList;
    }

    public VarDecl getFuncId() {
        return funcId;
    }

    public FormalDeclList getFuncArg() {
        return funcArg;
    }

    public StmtList getStmtList(){
        return stmtList;
    }

    @Override
    public String toString(){
        return this.funcId.toString() + "(" + this.funcArg.toString() + ") \n\r\t" + this.stmtList.toString();
    }

}
