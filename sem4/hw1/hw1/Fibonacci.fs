module Fibonacci

open System

let fibonacci n =
    let rec recursiveFibonacci i firstN secondN =
        match n with
        | _ when n < 0 -> raise (ArgumentOutOfRangeException())
        | _ when n >= 0 && n < 2 -> n
        | index when index = i -> firstN
        | _ -> recursiveFibonacci (i + 1) secondN (firstN + secondN)
    recursiveFibonacci 0 0 1