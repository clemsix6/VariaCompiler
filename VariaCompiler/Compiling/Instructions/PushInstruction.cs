namespace VariaCompiler.Compiling.Instructions;

public class PushInstruction : Instruction
{
    public Ptr Element { get; }


    public PushInstruction(Ptr element)
    {
        this.Element = element;
    }


    public PushInstruction(Ptr element, string comment) : base(comment)
    {
        this.Element = element;
    }


    public override string ToString()
    {
        return AppendComment($"\tpush\t{this.Element}");
    }
}