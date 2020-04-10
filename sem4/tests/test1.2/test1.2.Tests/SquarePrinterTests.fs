module test1._2.Tests

open NUnit.Framework
open FsUnit
open SquarePrinter
open System

[<TestCase(0)>]
[<TestCase(-1)>]
[<TestCase(-5)>]
[<TestCase(-8)>]
[<Test>]
let incorrectInputTest n =
    (fun () -> printSquare(n) |> ignore) |> should throw typeof<InvalidOperationException>
