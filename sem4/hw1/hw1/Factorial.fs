module Factorial

open System

let factorial n =
    match n with
    | _ when n < 0 -> raise (ArgumentOutOfRangeException())
    | _ ->
        let rec recursiveFactorial n acc =
            match n with
            | 0 | 1 -> acc
            | _ -> recursiveFactorial (n - 1) (acc * n)  
        recursiveFactorial n 1