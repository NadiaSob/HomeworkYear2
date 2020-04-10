module test1.Tests

open NUnit.Framework
open FsUnit
open MinElement
open System

let testCases () =
    [
        [2], 2
        [0], 0
        [-1], -1
        [2; 1], 1
        [-2; -222], -222
        [3; 5; 7; 235; 653; 99; 8765], 3
        [8; 42; 777; 1], 1
        [124; 436; 5673; 5322; 0; -34; -1266; 35], -1266
    ] |> List.map (fun (list, res) -> TestCaseData(list, res))

[<TestCaseSource("testCases")>]
[<Test>]
let findMinTest list res =
    findMin(list) |> should equal res

[<Test>]
let emptyListTest () =
    (fun () -> findMin([]) |> ignore) |> should throw typeof<InvalidOperationException>
