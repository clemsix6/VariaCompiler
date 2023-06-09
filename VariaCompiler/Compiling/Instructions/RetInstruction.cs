namespace VariaCompiler.Compiling.Instructions;

public class RetInstruction : Instruction
{
    public RetInstruction() { }


    public RetInstruction(string comment) : base(comment) { }


    public override void Build(Function function, List<Instruction> instructions) { }


    public override string ToString()
    {
        return AppendComment("\tret");
    }
}