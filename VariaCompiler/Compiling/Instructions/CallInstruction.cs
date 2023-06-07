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


    public override string ToString()
    {
        return AppendComment($"\tcall\t{this.Name}");
    }
}