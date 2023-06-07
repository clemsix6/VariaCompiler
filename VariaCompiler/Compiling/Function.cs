using System.Text;
using VariaCompiler.Compiling.Instructions;
using VariaCompiler.Compiling.Instructions.Operations;
using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling;

public partial class Function
{
    public FunctionDeclarationNode Declaration { get; }

    private int               _tempCounter = 1;
    private List<Instruction> _instructions;
    private List<Variable>    _variables;
    private List<Function>    _functions;


    public Function(FunctionDeclarationNode declaration, List<Function> functions)
    {
        this.Declaration   = declaration;
        this._instructions = new List<Instruction>();
        this._variables    = new List<Variable>();
        this._functions    = functions;

        AddHeader();
        Visit(declaration.Body);
        MoveStack();
    }


    private void AddHeader()
    {
        var name = this.Declaration.Name.Value;
        if (name == "main") this._instructions.Add(new GlobalInstruction(name));
        this._instructions.Add(new ProcedureInstruction(name));
        this._instructions.Add(new PushInstruction(new Register(Words.RegisterType.BP)));
        this._instructions.Add(
            new MovInstruction(new Register(Words.RegisterType.BP), new Register(Words.RegisterType.SP))
        );
    }


    private void MoveStack()
    {
        var stackSize = this._variables.Sum(x => x.Size);
        stackSize = (int) Math.Ceiling(stackSize / 16.0) * 16;
        this._instructions.Insert(
            3,
            new OperationInstruction(new Register(Words.RegisterType.SP), new Number(stackSize), "sub", "Move stack")
        );
        this._instructions.Insert(
            this._instructions.Count - 2,
            new OperationInstruction(new Register(Words.RegisterType.SP), new Number(stackSize), "add", "Restore stack")
        );
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


    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var instruction in this._instructions) builder.AppendLine(instruction.ToString());
        return builder.ToString();
    }
}