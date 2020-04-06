module EvenNumbersCount

/// Counts even numbers in the list using map function.
let evenNumbersCountMap list =
    list |> List.map (fun n -> abs((n + 1) % 2)) |> List.sum

/// Counts even numbers in the list using filter function.
let evenNumbersCountFilter list =
    list |> List.filter (fun n -> n % 2 = 0) |> List.length

/// Counts even numbers in the list using fold function.
let evenNumbersCountFold list = 
    list |> List.fold (fun acc n -> abs((n + 1) % 2) + acc) 0