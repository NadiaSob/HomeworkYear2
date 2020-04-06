module MapForBinaryTree

/// Binary tree type.
type Tree<'a> =
    | Node of 'a * Tree<'a> * Tree<'a>
    | Empty

/// Applies the function to each element of the binary tree and returns a new binary tree.
let rec map func tree = 
    match tree with
    | Node(root, left, right) -> Node(func root, map func left, map func right)
    | Empty -> Empty