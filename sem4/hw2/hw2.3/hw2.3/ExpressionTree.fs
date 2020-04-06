module ExpressionTree

/// Parsing tree for an arithmetic expression type.
type ExpressionTree = 
    | Number of int
    | Addition of ExpressionTree * ExpressionTree
    | Subtraction of ExpressionTree * ExpressionTree
    | Multiplication of ExpressionTree * ExpressionTree
    | Division of ExpressionTree * ExpressionTree

/// Calculates the parsing tree value of an arithmetic expression.
let rec calculate tree =
    match tree with
    | Number n -> n
    | Addition(treeL, treeR) -> calculate(treeL) + calculate(treeR)
    | Subtraction(treeL, treeR) -> calculate(treeL) - calculate(treeR)
    | Multiplication(treeL, treeR) -> calculate(treeL) * calculate(treeR)
    | Division(treeL, treeR) -> calculate(treeL) / calculate(treeR)