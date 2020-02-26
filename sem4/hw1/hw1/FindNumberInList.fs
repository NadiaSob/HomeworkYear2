module FindNumberInList

let findNumberInList list n =
    let rec recursiveFindNumber list i =
        match list with
        | [] -> None
        | head :: tail when head = n -> Some(i)
        | _ -> recursiveFindNumber list.Tail (i + 1)
    recursiveFindNumber list 0