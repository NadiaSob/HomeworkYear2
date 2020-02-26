module Fibonacci

open System

let fibonacci n =
    match n with
    | _ when n < 0 -> raise (ArgumentOutOfRangeException())
    | _ ->
        let rec recursiveFibonacci i firstN secondN =
            match n with
            | 0 | 1 -> n
            | index when index = i -> firstN
            | _ -> recursiveFibonacci (i + 1) secondN (firstN + secondN)
        recursiveFibonacci 0 0 1