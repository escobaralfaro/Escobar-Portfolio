package ast;

public class IfElseStmt extends Stmt {
    final Cond condition;
    final Stmt stmt1;
    final Stmt stmt2;

    public IfElseStmt(Cond condition, Stmt stmt1, Stmt stmt2, Location loc) {
        super(loc);
        this.stmt1 = stmt1;
        this.stmt2 = stmt2;
        this.condition = condition;
    }

    public Cond getCondition(){
        return this.condition;
    }

    public Stmt getLeftStmt(){
        return this.stmt1;
    }

    public Stmt getRightStmt(){
        return this.stmt2;
    }

    @Override
    public String toString(){
        return "if (" + this.condition + "){ \n\r\t" + this.stmt1 + "\n\r\t} else{ " + "\n\r\t" + this.stmt2 + "\n\r\t}"; 
    }

}
