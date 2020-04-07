module hw2._4.Tests

open NUnit.Framework
open FsUnit
open PrimeNumbersGenerator

[<TestCase(0, 2)>]
[<TestCase(1, 3)>]
[<TestCase(3, 7)>]
[<TestCase(4, 11)>]
[<TestCase(5, 13)>]
[<TestCase(9, 29)>]
[<TestCase(10, 31)>]
[<TestCase(11, 37)>]
[<TestCase(18, 67)>]
[<TestCase(25, 101)>]
[<TestCase(34, 149)>]
[<TestCase(46, 211)>]
[<Test>]
let primeNumbersGeneratorTest index n =
    generatePrimeNumbers () |> Seq.item index |> should equal n
