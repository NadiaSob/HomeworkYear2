module MapForBinaryTree

type Tree<'a> =
    | Node of 'a * Tree<'a> * Tree<'a>
    | Empty

let rec map func tree = 
    match tree with
    | Node(root, left, right) -> Node(func root, map func left, map func right)
    | Empty -> Empty