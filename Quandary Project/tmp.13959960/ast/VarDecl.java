package ast;

public class VarDecl extends ASTNode {

    final Type type;
    final String id;
    public final Boolean isMutable;

    public VarDecl(Boolean isMutable, Type varType, String Ident, Location loc) {
        super(loc);
        this.type = varType;
        this.id = Ident;
        this.isMutable = isMutable;
    }

    public Type getType(){
        return this.type;
    }

    public String getName(){
        return this.id + "";
    }

    public Boolean isMutable(){
        return this.isMutable;
    }

    @Override
    public String toString(){
        return type.toString() + " " + id;
    }

}