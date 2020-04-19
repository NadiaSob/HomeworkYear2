module FindNumberInList.Tests

open NUnit.Framework
open FindNumberInList

let testCases =
    [
        Some(0), [1; 2], 1
        Some(2), [4; 8; 16; 32], 16
        Some(6), [32; 64; 128; 256; 512; 1024; 2048; 4096; 8192; 16384], 2048
        Some(200), [1 .. 300], 201
    ] |> List.map (fun (index, list, n) -> TestCaseData(index, list, n))

[<TestCaseSource("testCases")>]
[<Test>]
let findNumberInListTest index list n =
    Assert.AreEqual(index, findNumberInList list n)

[<Test>]
let noNumberInListTest =
    Assert.AreEqual(None, findNumberInList [-50 .. 50] 51)
    Assert.AreEqual(None, findNumberInList [] 1)