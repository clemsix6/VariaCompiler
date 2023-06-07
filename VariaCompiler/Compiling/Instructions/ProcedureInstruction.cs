namespace VariaCompiler.Compiling.Instructions;

public class ProcedureInstruction : Instruction
{
    public string Name { get; }


    public ProcedureInstruction(string name)
    {
        this.Name = name;
    }


    public ProcedureInstruction(string name, string comment) : base(comment)
    {
        this.Name = name;
    }


    public override string ToString()
    {
        return AppendComment($"{this.Name}:");
    }
}