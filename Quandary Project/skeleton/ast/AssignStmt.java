package ast;

// public class AssignStmt extends Stmt {

//     final Expr right;
//     final VarDecl left;

//     public AssignStmt(VarDecl varDecl, Expr expr, Location loc) {
//         super(loc);
//         this.right = expr;
//         this.left = varDecl;
//     }

//     public Expr getExpr(){
//         return this.right;
//     }

//     public VarDecl getVarDecl(){
//         return this.left;
//     }

//     @Override
//     public String toString(){
//         return left + " = " + right;
//     }

// }
 
 public class AssignStmt extends Stmt {
 
     final String ident;
     final Expr rValue;
 
     public AssignStmt(String ident, Expr expr, Location loc) {
         super(loc);
         this.rValue = expr;
         this.ident = ident;
     }
 
     public Expr getExpr(){
         return this.rValue;
     }
 
     public String getIdent(){
         return this.ident;
     }
 
     @Override
     public String toString(){
         return this.ident + " = " + this.rValue;
     }
 
 }