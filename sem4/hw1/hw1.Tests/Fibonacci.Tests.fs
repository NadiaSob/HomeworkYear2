module Fibonacci.Tests

open NUnit.Framework
open Fibonacci

[<TestCase(1, 1)>]
[<TestCase(0, 0)>]
[<TestCase(1, 2)>]
[<TestCase(2, 3)>]
[<TestCase(8, 6)>]
[<TestCase(55, 10)>]
[<TestCase(4181, 19)>]
[<Test>]
let fibonacciTest result  number =
    Assert.AreEqual(result, fibonacci number)