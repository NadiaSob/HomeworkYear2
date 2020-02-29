module Factorial.Tests

open NUnit.Framework
open Factorial
open System

[<TestCase(1, 1)>]
[<TestCase(1, 0)>]
[<TestCase(2, 2)>]
[<TestCase(6, 3)>]
[<TestCase(720, 6)>]
[<TestCase(40320, 8)>]
[<TestCase(3628800, 10)>]
[<Test>]
let factorialTest result number =
    Assert.AreEqual(result, factorial number)

[<TestCase(-1)>]
[<TestCase(-10)>]
[<TestCase(-12345)>]
[<Test>]
let incorrectInputFactorialTest number =
    Assert.Throws<ArgumentOutOfRangeException>(fun () -> factorial number |> ignore) |> ignore