module FindNumberInList

let findNumberInList list n =
    let rec recursiveFindNumber list i =
        match list with
        | [] -> None
        | _ when n = list.Head -> Some(i)
        | _ -> recursiveFindNumber list.Tail (i + 1)
    recursiveFindNumber list 0