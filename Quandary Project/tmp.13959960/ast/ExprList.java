package ast;

public class ExprList extends ASTNode {

    final Expr expr;
    final ExprList exprList;

    public ExprList(Expr e, ExprList el, Location loc) {
        super(loc);
        this.expr = e;
        this.exprList = el;
    }

    public Expr getExpr() {
        return this.expr;
    }
    
    public ExprList getExprList() {
        return this.exprList;
    }

    // public boolean isEmpty() {
    //     return this.expr == null;  // Could also check `exprList == null`
    // }

}