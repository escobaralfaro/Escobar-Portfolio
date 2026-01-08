package ast;

public class IfStmt extends Stmt {
    final Cond condition;
    final Stmt stmt;

    public IfStmt(Cond condition, Stmt stmt, Location loc) {
        super(loc);
        this.condition = condition;
        this.stmt = stmt;
    }

    public Cond getCondition(){
        return this.condition;
    }

    public Stmt getStmt(){
        return this.stmt;
    }

    @Override
    public String toString(){
        return "if (" + this.condition + ") { \n\r\t" + this.stmt + "\n\r\t}";
    }

}
