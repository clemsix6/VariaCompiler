namespace VariaCompiler.Compiling.Instructions.Operations;

public class OperationInstruction : Instruction
{
    public Ptr    Destination { get; }
    public Ptr    Source      { get; }
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


    public override string ToString()
    {
        return AppendComment($"\t{this.Operation}\t{this.Destination}, {this.Source}");
    }
}