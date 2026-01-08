package ast;

public class LogicCondition extends Cond {
    
    public static final int AND = 1;
    public static final int OR = 2;
    public static final int NOT = 3;

    final Cond cond1;
    final int operator; 
    final Cond cond2;

    public LogicCondition(Cond cond1, int operator, Cond cond2, Location loc) {
        super(loc);
        this.cond1 = cond1;
        this.cond2 = cond2;
        this.operator = operator;
    }

    public Cond getLeftCond() {
        return this.cond1;
    }

    public Cond getRightCond() {
        return this.cond2;
    }

    public int getOperator(){
        return this.operator;
    }

    @Override
    public String toString(){
        String s = null;
        switch (this.operator) {
            case AND:  s = "&&"; break;
            case OR: s = "||"; break;
            case NOT: s = "!"; break;
        }
        return cond1 + " " + s + " " + cond2;
    }

}
