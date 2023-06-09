namespace VariaCompiler.Compiling.Instructions;

public class CallInstruction : Instruction
{
    public string Name { get; }


    public CallInstruction(string name)
    {
        this.Name = name;
    }


    public CallInstruction(string name, string comment) : base(comment)
    {
        this.Name = name;
    }


    private void AddAutoComment()
    {
        this.Comment = $"Call {this.Name} function";
    }


    public override void Build(Function function, List<Instruction> instructions) { }


    public override string ToString()
    {
        if (this.Comment == null) AddAutoComment();
        return AppendComment($"\tcall\t{this.Name}");
    }
}