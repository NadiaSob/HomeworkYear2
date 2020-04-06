module hw2._2.Tests

open NUnit.Framework
open FsUnit
open MapForBinaryTree

[<Test>]
let mapForBinaryTreeTest () =
    let tree = (Node(4, Empty, Empty))
    let res = (Node(16, Empty, Empty))
    map (fun n -> n * n) tree |> should equal res

    let tree = (Node(5, Node(55, Node(105, Empty, Empty), Empty), Node(-10, Empty, Empty)))
    let res = (Node(10, Node(60, Node(110, Empty, Empty), Empty), Node(-5, Empty, Empty)))
    map (fun n -> n + 5) tree |> should equal res

    let tree = (Node("", Node(" ", Empty, Node("TestStr", Empty, Empty)), Empty))
    let res = (Node("Lala", Node(" Lala", Empty, Node("TestStrLala", Empty, Empty)), Empty))
    map (fun str -> str + "Lala") tree |> should equal res

[<Test>]
let mapForEmptyTreeTest () =
    map (fun n -> n - 100) Empty = Empty |> should be True