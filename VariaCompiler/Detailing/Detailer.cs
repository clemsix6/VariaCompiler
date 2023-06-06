using System.Text;
using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Detailing;

public class Detailer
{
    private StringBuilder _builder;
    private int           _tempCounter = 1;

    private FunctionDeclarationNode                                      currentFunction;
    private Dictionary<FunctionDeclarationNode, Dictionary<string, int>> _functionVariables = new();


    public string Compile(Node node)
    {
        this._builder = new StringBuilder();
        AddHeader();
        Visit(node);
        return this._builder.ToString();
    }


    private void AddHeader()
    {
        AppendLine(".globl  main", "Add global main\n");
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
                this.currentFunction = function;
                this._functionVariables.Add(function, new Dictionary<string, int>());
                Visit(function);
                break;
            case ReturnNode returnNode:
                Visit(returnNode);
                break;
            case BlockNode block:
                Visit(block);
                break;
            case AssignmentNode assignment:
                Visit(assignment);
                break;
            default: throw new Exception("Unknown node type");
        }
    }


    private string Visit(OperatorNode op, AssignmentNode? assignmentNode = null)
    {
        string? left = null;
        if (op.Left is OperatorNode opNode1)
            left = $"\tmov eax, {GetVariableStack(Visit(opNode1))}";
        else if (op.Left is NumberNode numNode1)
            left = $"\tmov eax, {numNode1.Token.Value}";
        else if (op.Left is IdentifierNode idNode1)
            left = $"\tmov eax, dword ptr [rbp-{this._functionVariables[this.currentFunction][idNode1.Name.Value]}]";
        else
            throw new Exception("Unsupported operand type");
        AppendLine("\n" + left, "Left side of operation");

        string? right = null;
        if (op.Right is OperatorNode opNode)
            left = $"\tmov eax, {GetVariableStack(Visit(opNode))}";
        else if (op.Right is NumberNode numNode)
            right = $"\tmov ebx, {numNode.Token.Value}";
        else if (op.Right is IdentifierNode idNode)
            right =
                $"\tmov ebx, dword ptr [rbp-{this._functionVariables[this.currentFunction][idNode.Name.Value]}]";
        else
            throw new Exception("Unsupported operand type");
        AppendLine(right, "Right side of operation");

        var operation = "";
        switch (op.OperatorToken.Value) {
            case "+":
                operation = "\tadd eax, ebx";
                break;
            case "-":
                operation = "\tsub eax, ebx";
                break;
            case "*":
                operation = "\timul eax, ebx";
                break;
            case "/":
                operation = "\tidiv ebx";
                break;
            default: throw new NotSupportedException($"The operator {op.OperatorToken.Value} is not supported");
        }

        AppendLine(operation);

        if (assignmentNode != null) {
            AppendLine(
                $"\tmov {GetVariableStack(assignmentNode.Name.Value)}, eax",
                $"Store the result in \"{assignmentNode.Name.Value}\""
            );
            return assignmentNode.Name.Value;
        }

        var tempName = $"__temp{this._tempCounter++}";
        CreateVariable(tempName, "eax");
        return tempName;
    }


    private void Visit(FunctionDeclarationNode function)
    {
        var name = function.Name.Value;
        AppendLine($"{name}:");
        AppendLine("\tpush rbp");
        AppendLine("\tmov rbp, rsp\n");
        Visit(function.Body);
        AppendLine("\n\tpop rbp");
        AppendLine("\tret");
    }


    private void Visit(ReturnNode returnNode)
    {
        var expression = returnNode.Expression;

        if (expression is NumberNode) {
            AppendLine($"\tmov eax, {(returnNode.Expression as NumberNode)?.Token.Value}");
        }
        else if (expression is IdentifierNode identifier) {
            var variables = this._functionVariables[this.currentFunction];
            var stack     = variables[identifier.Name.Value];
            AppendLine(
                $"\n\tmov eax, dword ptr [rbp-{stack}]",
                $"Copy value of \"{identifier.Name.Value}\" to eax for return"
            );
        }
    }


    private void Visit(BlockNode block)
    {
        foreach (var statement in block.Statements) Visit(statement);
    }


    private void Visit(AssignmentNode assignment)
    {
        var expression = assignment.Expression;
        var variables  = this._functionVariables[this.currentFunction];

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
        }
    }


    private bool CreateVariable(string name, string? value = null)
    {
        var variables = this._functionVariables[this.currentFunction];
        var stack     = variables.Any() ? variables.Max(x => x.Value) + 4 : 4;
        if (!variables.TryAdd(name, stack)) return false;
        if (value != null)
            AppendLine(
                $"\tmov dword ptr [rbp-{stack}], {value}",
                $"Create variable \"{name}\" and assign {value} to it"
            );
        else
            AppendLine(
                $"\tmov dword ptr [rbp-{stack}], 0",
                $"Create variable \"{name}\" and assign 0 to it"
            );
        return true;
    }


    private string GetVariableStack(string name)
    {
        var variables = this._functionVariables[this.currentFunction];
        var stack     = variables[name];
        return $"dword ptr [rbp-{stack}]";
    }


    private string? GetVariableName(int stack)
    {
        var variables = this._functionVariables[this.currentFunction];
        return variables.FirstOrDefault(x => x.Value == stack).Key;
    }


    private void AppendLine(string line, string? comment = null)
    {
        if (comment == null) {
            this._builder.AppendLine(line);
            return;
        }

        var len = line.Where(x => x != '\n').Select(x => x == '\t' ? 8 : 1).Sum();
        this._builder.AppendLine($"{line}{new string(' ', 50 - len)}# {comment}");
    }
}