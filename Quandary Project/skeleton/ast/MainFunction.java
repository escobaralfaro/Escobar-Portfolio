package ast;

public class MainFunction extends ASTNode {
    final VarDecl funcId;
    final VarDecl funcArg;
    final StmtList stmtList;

    public MainFunction(VarDecl funcId, VarDecl funcArg, StmtList stmtList, Location loc) {
        super(loc);
        this.funcId = funcId;
        this.funcArg = funcArg;
        this.stmtList = stmtList;
    }

    public VarDecl getFuncId() {
        return funcId;
    }

    public VarDecl getFuncArg() {
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