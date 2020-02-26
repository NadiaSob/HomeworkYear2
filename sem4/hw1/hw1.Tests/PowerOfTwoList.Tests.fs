module PowerOfTwoList.Tests

open NUnit.Framework
open PowerOfTwoList
open System

let testCases =
    [
        [1; 2], 0, 1
        [4; 8; 16; 32], 2, 3
        [32; 64; 128; 256; 512; 1024; 2048; 4096; 8192; 16384], 5, 9
        [16384], 14, 0
    ] |> List.map (fun (list, n, m) -> TestCaseData(list, n, m))

[<TestCaseSource("testCases")>]
[<Test>]
let powerOfTwoListTest list n m =
    Assert.AreEqual(list, powerOfTwoList n m)

[<TestCase(-1, 4)>]
[<TestCase(6, -7)>]
[<TestCase(-12, -5)>]
[<TestCase(-123, -123)>]
[<Test>]
let incorrectInputFibonacciTest n m =
    Assert.Throws<ArgumentOutOfRangeException>(fun () -> powerOfTwoList n m |> ignore) |> ignore