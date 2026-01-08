package ast;

public class UnaryExpr extends Expr {

    public static final int UMINUS = 1;
    final Expr expr;
    final int operator;

    public UnaryExpr(int operator, Expr expr, Location loc) {
        super(loc);
        this.expr = expr;
        this.operator = operator;
    }

    public Expr getExpr() {
        return expr;
    }

    public int getOperator() {
        return operator;
    }

    @Override
    public String toString() {
        return "- " + expr;
    }
}
