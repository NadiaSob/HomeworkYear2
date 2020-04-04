module EvenNumbersCount

let evenNumbersCountMap list =
    list |> List.map (fun n -> abs((n + 1) % 2)) |> List.sum

let evenNumbersCountFilter list =
    list |> List.filter (fun n -> n % 2 = 0) |> List.length

let evenNumbersCountFold list = 
    list |> List.fold (fun acc n -> abs((n + 1) % 2) + acc) 0