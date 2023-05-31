using System.Text;
using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Detailing;

public class Detailer
{
    private StringBuilder _builder;
    private int           _tempCounter = 1;


    public string Compile(Node node)
    {
        this._builder = new StringBuilder();
        Visit(node);
        return this._builder.ToString();
    }


    private void Visit(Node node)
    {
        switch (node) {
            case NumberNode number:
                Visit(number);
                break;
            case OperatorNode op:
                Visit(op);
                break;
            case FunctionDeclarationNode function:
                Visit(function);
                break;
            case ReturnNode returnNode:
                Visit(returnNode);
                break;
            case BlockNode block:
                Visit(block);
                break;
            default: throw new Exception("Unknown node type");
        }
    }


    private string Visit(OperatorNode op)
    {
        var left = op.Left is OperatorNode
                       ? Visit(op.Left as OperatorNode)
                       : $"mov rax, {(op.Left as NumberNode).Token.Value}";
        this._builder.AppendLine(left);

        var right = op.Right is OperatorNode
                        ? Visit(op.Right as OperatorNode)
                        : $"mov rbx, {(op.Right as NumberNode).Token.Value}";
        this._builder.AppendLine(right);

        var operation = "";
        switch (op.OperatorToken.Value) {
            case "+":
                operation = "add rax, rbx";
                break;
            case "-":
                operation = "sub rax, rbx";
                break;
            case "*":
                operation = "imul rbx";
                break;
            case "/":
                this._builder.AppendLine("xor rdx, rdx"); // clear rdx before division
                operation = "idiv rbx";
                break;
            default: throw new NotSupportedException($"The operator {op.OperatorToken.Value} is not supported");
        }

        this._builder.AppendLine(operation);
        return operation;
    }


    private void Visit(FunctionDeclarationNode function)
    {
        this._builder.Append($"{function.Name.Value}:\n");

        // Function prologue
        this._builder.Append("push rbp\n");
        this._builder.Append("mov rbp, rsp\n");

        Visit(function.Body);

        // Function epilogue
        this._builder.Append("mov rsp, rbp\n");
        this._builder.Append("pop rbp\n");

        this._builder.Append("ret\n");
    }


    private void Visit(ReturnNode returnNode)
    {
        var expression = returnNode.Expression is OperatorNode
                             ? Visit(returnNode.Expression as OperatorNode)
                             : $"mov rax, {(returnNode.Expression as NumberNode).Token.Value}";

        this._builder.Append($"{expression}\n");
    }


    private void Visit(BlockNode block)
    {
        foreach (var statement in block.Statements) Visit(statement);
    }
}