module ReverseList

let reverseList list =
       let rec recursiveReverseList list reversedList =
           match list with
           | [] -> reversedList
           | _ -> recursiveReverseList list.Tail (list.Head :: reversedList)
       recursiveReverseList list []