module ReverseList

let reverseList list =
    let rec recursiveReverseList list reversedList =
        match list with
        | [] -> reversedList
        | head :: tail -> recursiveReverseList tail (head :: reversedList)
    recursiveReverseList list []