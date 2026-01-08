/* ./ref/quandary ref/notes.q arg */

Q main(int arg) {
    /*return apply(map(makeList (arg), square), add);*/
    /*return length(makeList(arg)); */
    /*return length(1.2.3.4.5.6.7.nil);*/
    /*return filter(makeList(arg), isEven); */
    return apply(map((Ref) filter(makeList(arg), isEven), square), add);
}

/* 1/31 */
int isEven(Q x) {
    if ((int)x < 0) return isEven((int)x * -1);
    if ((int)x == 0) return 1;
    if ((int)x == 1) return 0;
    return isEven((int) x - 2);
}

Q filter (Ref list, Q f) {
    if (isNil(list) == 1) return nil;
    Q left = left(list);
    if (f(left == 1)) return left . filter((Ref) right(list), f);
    return filter((Ref)right(list), f)
}

Q length(Ref list) {
    /* add 0 */
    /*if nil --> 0 */
    /*if list with (Q . littlelist) --> 1 + recursion */
    return apply(map(list, theOne), add);
}

int theOne(Q x) {
    return 1;
}

/* 1/29 */
Q add (Q x, Q y){
    return (int)x + (int) y;
}
Q prod (Q x, Q y){
    return (int)x * (int) y;
}
Q apply(Ref list, Q f){
    if (isNil(right(list)) == 1) return left(list);
    return f(left(list), apply((Ref) right(list), f));
}
int sumAll(Ref list){
    if(isNil(list) == 1) return 0;
    return (int)left (list) + sumAll((Ref) right(list));
}
int fact(Ref list){
    if(isNil(list) == 1) return 1;
    return (int)left (list) * fact((Ref) right(list));
}
Q double (Q x){
    return (int)x * 2;
}

Q square(Q x){
    return (int)x * (int) x;
}

Ref map(Ref list, Q f){
    if(isNil(list) == 1) return list;
    return f(left (list)) . map((Ref) right(list), f);
}

Ref doubleAll(Ref list){
    if(isNil(list) == 1) return list;
    return (int)left(list) + (int)left (list) . doubleAll((Ref) right(list));
}

Ref squareAll(Ref list){
    if(isNil(list) == 1) return list;
    return (int)left(list) * (int)left (list) . squareAll((Ref) right(list));
}
/* doubles?
 * squares
 * map
 * sum?
 * product?
 * apply?
 * doubles divisible by 3?
 * filter?
 * combinations?
 */

Q steve (int n){
    if (n == 0) return n;
    return steve (n-1) . steve (n-1);
}

Ref makeList (int n){
/*    if (n == 0) return nil;
    return n . makeList (n-1);*/
    return bs (1, n);
}

Ref flip (Ref x){
    if (isNil(x) == 1) return nil;
    return flip((Ref) right(x)) . left(x);
}

Ref bs(int current, int length){
    if (current > length) return nil;
    return current . bs(current + 1, length);
}

int fib (int x) {
    /* good condition */
    if (x <= 1) return x;
    return fib(x -1) + fib (x -2);
}

int betterFib (int current, int last, int count){
    if (count == 0) return current;
    return betterFib (current+last, current, count -1);
}