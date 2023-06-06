using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling;

public partial class Function
{
    private void Visit(ReturnNode returnNode)
    {
        var expression = returnNode.Expression;

        switch (expression) {
            case NumberNode:
            {
                AppendLine($"\tmov eax, {(returnNode.Expression as NumberNode)?.Token.Value}");
                break;
            }
            case IdentifierNode identifier:
            {
                var stack = GetVariableStack(identifier.Name.Value);
                AppendLine(
                    $"\n\tmov eax, {stack}",
                    $"Copy value of \"{identifier.Name.Value}\" to eax for return"
                );
                break;
            }
            case OperatorNode opNode:
            {
                Visit(opNode, null, true);
                break;
            }
            case FunctionCallNode functionCall:
            {
                Visit(functionCall);
                break;
            }
        }

        AppendLine("\n\tpop rbp");
        AppendLine("\tret");
    }
}