module PowerOfTwoList

open System
open ReverseList

let powerOfTwoList n m =
    match m with
    | _ when m < 0 -> raise (ArgumentOutOfRangeException())
    | _ -> 
        match n with
        | _ when n < 0 -> raise (ArgumentOutOfRangeException())
        | _ ->
            let rec powerOfTwo x n =
                match n with
                | 0 -> x
                | _ -> powerOfTwo (x * 2) (n - 1) 

            let rec recursivePowerOfTwoList list pow i =
                match i with
                | lastPow when lastPow = m -> list
                | _ -> recursivePowerOfTwoList ((pow * 2) :: list) (pow * 2) (i + 1)
            let nPoverOfTwo = powerOfTwo 1 n
            reverseList (recursivePowerOfTwoList [nPoverOfTwo] (nPoverOfTwo) 0)