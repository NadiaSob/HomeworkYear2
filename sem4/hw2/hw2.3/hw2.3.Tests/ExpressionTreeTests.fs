module hw2._3.Tests

open NUnit.Framework
open FsUnit
open ExpressionTree

let testCases () =
    [
        Number(5), 5
        Number(-1), -1
        Addition(Number(4), Number(5)), 9
        Addition(Number(245), Number(-575)), -330
        Subtraction(Number(7), Number(6)), 1
        Subtraction(Number(-53789), Number(-53000)), -789
        Multiplication(Number(5), Number(7)), 35
        Multiplication(Number(-444), Number(-23)), 10212
        Division(Number(8), Number(2)), 4
        Division(Number(-999), Number(9)), -111
        Addition(Number(7), Subtraction(Number(55), Number(793))), -731
        Subtraction(Multiplication(Number(6), Number(0)), Division(Number(-8000), Number(80))), 100
        Multiplication(Division(Number(430), Number(43)), Addition(Subtraction(Number(106), Number(6)), Number(10))), 1100
    ] |> Seq.map (fun (tree, res) -> TestCaseData(tree, res))

[<TestCaseSource("testCases")>]
[<Test>]
let expressionTreeCalculationTest tree res =
    calculate(tree) |> should equal res