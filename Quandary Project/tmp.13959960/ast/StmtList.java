package ast;

public class StmtList extends Stmt {

    private final Stmt stmt;
    private final StmtList sl;

    public StmtList(Stmt stmt, StmtList sl, Location loc) {
        super(loc);
        this.stmt = stmt;
        this.sl = sl;
    }

    // Base case
    public StmtList getStmtList() {
        return sl;
    }

    public Stmt getStmt() {
        return stmt;
    }
}