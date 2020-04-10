module MinElement

open System

/// Finds minimum element in the given list.
let findMin list =
    match list with
    | [] -> raise (InvalidOperationException())
    | _ ->
        let checkIfMin min n =
            match n with
            | _ when n < min -> n
            | _ -> min
        list |> List.fold checkIfMin list.Head