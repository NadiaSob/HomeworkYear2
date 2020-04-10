module SquarePrinter

open System

/// Prints square with a side length of n.
let printSquare n =
    match n with
    | _ when n < 1 -> raise (InvalidOperationException())
    | 1 -> printf "*"
    | _ ->
        let rec printEdge n acc =
            match acc with
            | _ when acc < n -> 
                printf "*"
                printEdge n (acc + 1)
            | _ -> printfn ""

        let rec printSide n acc = 
            let rec printBlank n acc =
                match acc with
                | _ when acc < n -> 
                    printf " "
                    printBlank n (acc + 1)
                | _ -> ()

            match acc with 
            | _ when acc < n -> 
                printf "*"
                printBlank n 0
                printfn "*"
                printSide n (acc + 1)
            | _ -> ()

        printEdge n 0
        printSide (n - 2) 0
        printEdge n 0
