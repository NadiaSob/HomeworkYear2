module hw2._1.Tests

open NUnit.Framework
open FsUnit
open FsCheck
open EvenNumbersCount

let testCases () =
    [
        [2], 1
        [0], 1
        [], 0
        [-1], 0
        [-2], 1
        [2; 1], 1
        [-2; -222], 2
        [1; 3; 5; 7; 235; -653; 99; 8765], 0
        [2; 4; 6; 8], 4
        [124; 436; 5673; 5322; 0; -34; -1266; 35], 6
    ] |> List.map (fun (list, res) -> TestCaseData(list, res))

[<TestCaseSource("testCases")>]
[<Test>]
let EvenNumbersCountMapTest list res =
    evenNumbersCountMap(list) |> should equal res

[<TestCaseSource("testCases")>]
[<Test>]
let EvenNumbersCountFilterTest list res =
    evenNumbersCountFilter(list) |> should equal res

[<TestCaseSource("testCases")>]
[<Test>]
let EvenNumbersCountFoldTest list res =
    evenNumbersCountFold(list) |> should equal res

let FunctionsEquals (list:list<int>) = 
    evenNumbersCountMap list = evenNumbersCountFilter list && evenNumbersCountFilter list = evenNumbersCountFold list

[<Test>]
Check.QuickThrowOnFailure FunctionsEquals
