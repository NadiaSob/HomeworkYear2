module ReverseList.Tests

open NUnit.Framework
open ReverseList

let testCases =
    [
        [1; 2], [2; 1]
        [5; 67; 45; 88; 123; 88; 96; 4; 0], [0; 4; 96; 88; 123; 88; 45; 67; 5]
        [], []
        [5], [5]
        [-5 .. 5], [5; 4; 3; 2; 1; 0; -1; -2; -3; -4; -5]
    ] |> List.map (fun (resList, list) -> TestCaseData(resList, list))

[<TestCaseSource("testCases")>]
[<Test>]
let reverseListTest resList list =
    Assert.AreEqual(resList, reverseList list)