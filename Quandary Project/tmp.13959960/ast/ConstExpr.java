package ast;

public class ConstExpr extends Expr {

    //final Object value;
    final QVal value;

    public ConstExpr(long value, Location loc) {
        super(loc);
        //this.value = value;
        this.value = new QIntVal(value);
    }

    public ConstExpr(Location loc) {
        super(loc);
        this.value = new QRefVal(true, null);
    }

    public QVal getValue() {
        return this.value;
    }
    public String toString() { 
        if (this.value != null)
             return value.toString();
         else
             return "nil";
    }
}
