package ast;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

public class QRefVal extends QVal {
    
    public QObj objVal;
    boolean isNil;
    boolean mutable;
    public final Lock lock; 

    public QRefVal(Boolean isNil, QObj value) {
        super(false);
        this.isNil = isNil;
        this.objVal = value;
        this.mutable = false;
        this.lock = new ReentrantLock(); 
    }

    public QRefVal(Boolean isMutable, Boolean isNil, QObj value) {
        super(isMutable);
        this.isNil = isNil;
        this.objVal = value;
        this.mutable = isMutable;
        this.lock = new ReentrantLock(); 
    }

    public boolean isMutable(){
        return this.mutable;
    }    

    public boolean isNil(){
        return this.isNil;
    }  

    public QVal getLeft(){
        System.out.println("Reading left of: " + this);
        return this.objVal.left;
    }

    public QVal getRight(){
        return this.objVal.right;
    }

    public Lock getLock() {
        return this.lock;
    }

    @Override
    public String toString() {
        if(objVal == null) {
            return "nil";
        }
        return objVal.toString();
    }
}
