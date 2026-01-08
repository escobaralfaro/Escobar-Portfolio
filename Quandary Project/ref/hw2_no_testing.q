/* #1 */
int isList(Q x) {
    /* nil is a list */
    if (isNil(x) != 0) {
        return 1;
    }

    /* if not a reference or nil, then not a list */
    if (isAtom(x) != 0) {
        return 0;
    }

    /* else check if right(x) is list */
    return isList(right((Ref)x));
}

/* #2 */
Ref append(Ref list1, Ref list2) {
    /* if list1 is nil, return list2 since done appending */
    if (isNil(list1) != 0) {
        return list2;
    }

    /* recursively append right of list1 to list2 */
    return left(list1) . append((Ref) right(list1), list2);
}

/* #3 */
Ref reverse(Ref list) {
    /* return list if nil */ 
    if (isNil(list) != 0) {
        return list;
    }

    /* add on left to end of reversed list */
    return append(reverse((Ref) right(list)), left(list) . nil);
}

/* #4 */
int length(Ref list) {
    /* if list is nil, length = 0 */
    if (isNil(list) != 0) {
        return 0;
    }

    /* recursively count down until nil */
    return 1 + length((Ref) right(list));
}

int isSorted(Ref list) {
    /* return sorted if empty or only one element */
    if (isNil(list) != 0) {
        return 1;
    }
    if (isNil(right(list)) != 0) {
        return 1;
    }

    /* get current len and next len */
    int len_curr = length((Ref)left(list));
    int len_next = length((Ref)left((Ref)right(list)));

    /* if earlier list longer than later list, then not sorted */
    if (len_curr > len_next) {
        return 0;
    }

    /* continue check rest of list */
    return isSorted((Ref)right(list));
}

/* #5 */
/* a) Quandary most likely cannot execute functions without if statements due to the fact that it would be missing a way to do the base case without an if statement. Without a way to make checks efficiently, there isn't really a way to determine state changes.*/

/* b) Functional programming like Quandary cannot operate without recursion because it does not have any other ways to change variables (no while or for loops). Hence if you remove recursion, then there is no way to perform unbounded calculations. Thus the code would only have minimal functionality, like performing basic checks up to a certain limit. */