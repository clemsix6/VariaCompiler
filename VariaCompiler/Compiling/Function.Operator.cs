using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling;

public partial class Function
{
    private string Visit(OperatorNode op, AssignmentNode? assignmentNode = null, bool eax = false)
    {
        var left  = GetOperand(op.Left,  "eax");
        var right = GetOperand(op.Right, "ebx");

        if (left  != null) AppendLine("\n" + left, "Left side of operation");
        if (right != null) AppendLine(right,       "Right side of operation");

        var operation = PerformOperation(op);
        AppendLine(operation);

        if (assignmentNode != null) return AssignResult(operation, assignmentNode);
        if (!eax) return TempResult(operation);
        return "eax";
    }


    private string? GetOperand(Node node, string register)
    {
        switch (node) {
            case OperatorNode opNode: return Visit(opNode);
            case NumberNode numNode:  return $"\tmov {register}, {numNode.Token.Value}";
            case IdentifierNode idNode1:
            {
                var stack = GetVariableStack(idNode1.Name.Value);
                return $"\tmov {register}, {stack}";
            }
            case FunctionCallNode functionCall:
            {
                Visit(functionCall);
                return null;
            }
            default: throw new Exception("Unsupported operand type");
        }
    }


    private string PerformOperation(OperatorNode op)
    {
        switch (op.OperatorToken.Value) {
            case "+": return "\tadd eax, ebx";
            case "-": return "\tsub eax, ebx";
            case "*": return "\timul eax, ebx";
            case "/": return "\tidiv ebx";
            default:  throw new NotSupportedException($"The operator {op.OperatorToken.Value} is not supported");
        }
    }


    private string AssignResult(string operation, AssignmentNode assignmentNode)
    {
        AppendLine(operation);
        AppendLine(
            $"\tmov {GetVariableStack(assignmentNode.Name.Value)}, eax",
            $"Store the result in \"{assignmentNode.Name.Value}\""
        );
        return assignmentNode.Name.Value;
    }


    private string TempResult(string operation)
    {
        var tempName = $"__temp{this._tempCounter++}";
        CreateVariable(tempName, "eax");
        return tempName;
    }
}