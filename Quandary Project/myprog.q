mutable Q main(int arg) {
    if (arg == 1) {
        
        return behaviour_1();

    } else if (arg == 2) {
        
        mutable int i = 15;
        while(i > 1){
            behaviour_2();
            i = i -1;
        }

    } else if (arg == 3) {
        
        return behaviour_3(16);

    } else if (arg == 4) {
        
        return behaviour_4(15);

    }
    
    return 0;
 }

// 5 = works 0 = doesnt work
mutable int behaviour_1() { 
    // MarkSweep -heapsize 384 return 5.   MarkSweep -heapsize 408 return 0
    Ref list = ben_list(15);
    return 1;
 }

mutable int behaviour_2() { 
    // RefCount -heapsize 384 return 5.   MarkSweep -heapsize 384 return 0 
    mutable Ref i = (3.nil);
    mutable Ref j = (2.nil);
    setRight(i, j);
    setRight(j, i);
    return 1;
 }

mutable int behaviour_3(mutable int n) {
    // MarkSweep -heapsize 384 return 5.   Explicit -heapsize 384 return 0 
    Ref list = (nil . nil);
    mutable Ref temp = list;

    while (n > 0) {
        setRight(temp, (nil . nil));
        Ref prev = temp;
        temp = (Ref)right(temp);
        free(prev);

        n = n - 1;
    }
    return 1;
 
 }

mutable int behaviour_4(mutable int n) {
    // Explicit -heapsize 384 return 5.  MarkSweep -heapsize 384 return 0
    while(n > 1){

    mutable Ref i = (3.nil);
    mutable Ref j = (2.nil);
    setRight(i, j);
    setRight(j, i);
    n = n -1;

    }

    return 1;
 }

Ref ben_list(int length) {
    if (length < 0) {
        return length.nil;
    }
    return length . ben_list(length - 1);
 }