using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling;

public partial class Function
{
    private void Visit(AssignmentNode assignment)
    {
        var expression = assignment.Expression;

        switch (expression) {
            case NumberNode numNode:
                CreateVariable(assignment.Name.Value, numNode.Token.Value);
                break;
            case IdentifierNode idNode:
            {
                CreateVariable(assignment.Name.Value, "eax");
                break;
            }
            case OperatorNode opNode:
            {
                Visit(opNode, assignment);
                CreateVariable(assignment.Name.Value, "eax");
                break;
            }
            case FunctionCallNode functionCall:
            {
                Visit(functionCall);
                CreateVariable(assignment.Name.Value, "eax");
                break;
            }
        }
    }
}