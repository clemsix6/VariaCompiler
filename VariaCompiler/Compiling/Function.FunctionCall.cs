using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling;

public partial class Function
{
    private void Visit(FunctionCallNode functionCall)
    {
        var args = new[] {"edi", "esi", "edx", "ecx", "r8d", "r9d"};

        HandleFunctionCallArguments(functionCall, args);
        HandleArgumentTypes(functionCall, args);

        AppendLine("\tcall " + functionCall.Name.Value);
    }


    private void HandleFunctionCallArguments(FunctionCallNode functionCall, string[] args)
    {
        for (var index = 0; index < functionCall.Arguments.Count; index++) {
            var argument = functionCall.Arguments[index];
            if (argument is not FunctionCallNode functionCallNode) continue;
            Visit(functionCallNode);
            AppendLine($"\tmov {args[index]}, eax");
        }
    }


    private void HandleArgumentTypes(FunctionCallNode functionCall, string[] args)
    {
        for (var index = 0; index < functionCall.Arguments.Count; index++) {
            var argument = functionCall.Arguments[index];
            switch (argument) {
                case NumberNode numNode:
                {
                    AppendLine($"\tmov {args[index]}, {numNode.Token.Value}");
                    break;
                }
                case IdentifierNode idNode:
                {
                    var stack = GetVariableStack(idNode.Name.Value);
                    AppendLine($"\tmov {args[index]}, {stack}");
                    break;
                }
                case OperatorNode operatorNode:
                {
                    Visit(operatorNode, null, true);
                    AppendLine($"\tmov {args[index]}, eax");
                    break;
                }
            }
        }
    }
}