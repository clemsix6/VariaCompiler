using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling.Instructions.Operations;

public class OperationInstruction : Instruction
{
    public Ptr    Destination { get; private set; }
    public Ptr    Source      { get; private set; }
    public string Operation   { get; }


    public OperationInstruction(Ptr destination, Ptr source, string operation)
    {
        this.Destination = destination;
        this.Source      = source;
        this.Operation   = ConvertOperation(operation);
    }


    public OperationInstruction(Ptr destination, Ptr source, string operation, string comment) : base(comment)
    {
        this.Destination = destination;
        this.Source      = source;
        this.Operation   = ConvertOperation(operation);
    }


    private string ConvertOperation(string operation)
    {
        switch (operation) {
            case "+":    return "add";
            case "-":    return "sub";
            case "*":    return "imul";
            case "/":    return "idiv";
            case "add":  return "add";
            case "sub":  return "sub";
            case "mul":  return "imul";
            case "imul": return "imul";
            case "div":  return "idiv";
            case "idiv": return "idiv";
            default:     throw new Exception($"Unknown operation: {operation}");
        }
    }


    private void AddAutoComment()
    {
        var source      = this.Source.GetIdentifier(true);
        var destination = this.Destination.GetIdentifier(true);

        switch (this.Operation) {
            case "add":
                this.Comment = $"Add {source} to {destination}";
                break;
            case "sub":
                this.Comment = $"Subtract {source} from {destination}";
                break;
            case "imul":
                this.Comment = $"Multiply {source} by {destination}";
                break;
            case "idiv":
                this.Comment = $"Divide {destination} by {source}";
                break;
            default: throw new Exception($"Unknown operation: {this.Operation}");
        }
    }


    private void AdjustSize(List<Instruction> instructions)
    {
        if (this.Source.Size == this.Destination.Size || this.Source is Number) return;
        if (this.Source is Register) {
            this.Source = new Register(Words.RegisterType.B, this.Destination.Size);
            return;
        }

        if (this.Source is Variable variable)
            this.Source = new Variable(variable.Name, variable.Type, variable.Stack, this.Destination.Size);
    }


    public override void Build(Function function, List<Instruction> instructions)
    {
        AdjustSize(instructions);
    }


    public override string ToString()
    {
        if (this.Comment == null) AddAutoComment();
        return AppendComment($"\t{this.Operation}\t{this.Destination}, {this.Source}");
    }
}