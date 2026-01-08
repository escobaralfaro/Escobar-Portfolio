package ast;

public class VarExpr extends Expr {

    final String indent;

    public VarExpr(String identifier, Location loc) {
        super(loc);
        this.indent = identifier;

    }

    public String getVarName() {
        return this.indent;
    }


    @Override
    public String toString() {
        return this.indent;
    }
}