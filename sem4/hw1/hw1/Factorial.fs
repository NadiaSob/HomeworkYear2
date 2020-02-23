module Factorial

open System

let factorial n =
    let rec recursiveFactorial n acc =
        match n with
        | _ when n < 0 -> raise (ArgumentOutOfRangeException())
        | 0 | 1 -> acc
        | _ -> recursiveFactorial (n - 1) (acc * n)  
    recursiveFactorial n 1