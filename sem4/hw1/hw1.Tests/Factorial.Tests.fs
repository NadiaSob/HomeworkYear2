module Factorial.Tests

open NUnit.Framework
open Factorial

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