using System.Text;
using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling;

public partial class Function
{
    private int                     _tempCounter = 1;
    private List<string>            _lines;
    private FunctionDeclarationNode _currentFunction;
    private List<Variable>          _variables;


    public Function(FunctionDeclarationNode function)
    {
        this._currentFunction = function;
        this._lines           = new List<string>();
        this._variables       = new List<Variable>();
        AddHeader();
        Visit(function.Body);
        MoveStack();
    }


    private void AddHeader()
    {
        var name = this._currentFunction.Name.Value;
        AppendLine($"{name}:");
        AppendLine("\tpush rbp");
        AppendLine("\tmov rbp, rsp\n");
    }


    private void MoveStack()
    {
        var stackSize = this._variables.Sum(x => x.Size);
        stackSize = (int) Math.Ceiling(stackSize / 16.0) * 16;
        InsertLine("\tsub rsp, " + stackSize, 3,                     "Move stack");
        InsertLine("\tadd rsp, " + stackSize, this._lines.Count - 2, "Restore stack");
    }


    public Node? Visit(Node node)
    {
        switch (node) {
            case FunctionDeclarationNode: return node;
            case OperatorNode op:
                Visit(op);
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
            case FunctionCallNode functionCall:
                Visit(functionCall);
                break;
            default: throw new Exception("Unknown node type");
        }

        return null;
    }


    private bool CreateVariable(string name, string? value = null)
    {
        var stack = this._variables.Any() ? this._variables.Max(x => x.Stack) + 4 : 4;
        if (this._variables.Any(x => x.Name == name)) return false;
        this._variables.Add(new Variable(name, stack, 4));
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


    private bool CreateVariable(string name, out int stack, string? value = null)
    {
        stack = this._variables.Any() ? this._variables.Max(x => x.Stack) + 4 : 4;
        if (this._variables.Any(x => x.Name == name)) return false;
        if (this._variables.Any(x => x.Name == name)) return false;
        this._variables.Add(new Variable(name, stack, 4));
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


    private string? GetVariableStack(string name)
    {
        Variable? variable;
        if ((variable = this._variables.Find(x => x.Name == name)) != null) return $"dword ptr [rbp-{variable.Stack}]";
        if (CreateVariable(name, out var stack)) return $"dword ptr [rbp-{stack}]";
        return null;
    }


    private void AppendLine(string line, string? comment = null)
    {
        if (comment == null) {
            this._lines.Add(line);
            return;
        }

        var len = line.Where(x => x != '\n').Select(x => x == '\t' ? 8 : 1).Sum();
        this._lines.Add($"{line}{new string(' ', 50 - len)}# {comment}");
    }


    private void InsertLine(string line, int index, string? comment = null)
    {
        if (comment == null) {
            this._lines.Insert(index, line + "\n");
            return;
        }

        var len = line.Where(x => x != '\n').Select(x => x == '\t' ? 8 : 1).Sum();
        this._lines.Insert(index, $"{line}{new string(' ', 50 - len)}# {comment}\n");
    }


    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var line in this._lines) builder.AppendLine(line);
        return builder.ToString();
    }
}