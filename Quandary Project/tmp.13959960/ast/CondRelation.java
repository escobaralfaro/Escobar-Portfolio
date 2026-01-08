package ast;

public class CondRelation extends Cond {
    
    public static final int LESSTH = 1;
    public static final int GREATERTH = 2;
    public static final int LESSEQ = 3;
    public static final int GREATEREQ = 4;
    public static final int EQ = 5;
    public static final int NOTEQ = 6;
    
    final Expr expr1;
    final int operator; 
    final Expr expr2;

    public CondRelation(Expr expr1, int operator, Expr expr2, Location loc) {
        super(loc);
        this.expr1 = expr1;
        this.expr2 = expr2;
        this.operator = operator;
    }

    public Expr getLeftExpr(){
        return this.expr1;
    } 

    public int getOperator(){
        return this.operator;
    }

    public Expr getRightExpr(){
        return this.expr2;
    } 

    @Override
    public String toString(){
        String s = null;
        switch (this.operator) {
            case LESSTH: s = "<"; break;
            case GREATERTH: s = ">"; break;
            case EQ: s = "=="; break;
            case NOTEQ: s = "!="; break;
            case LESSEQ: s = "<="; break;
            case GREATEREQ: s = ">="; break;
        }
        return expr1 + " " + s + " " + expr2;
    }

}
