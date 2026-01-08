package ast;

public class IdentExpr extends Expr {
    final String id;

    public IdentExpr(String id, Location loc) {
        super(loc);
        this.id = id;
    }

    public String getName() {
        return this.id;
    }

    @Override
    public String toString() {
        return this.id;
    }
}
