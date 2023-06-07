namespace VariaCompiler.Compiling.Instructions;

public class PopInstruction : Instruction
{
    public Ptr Element { get; }


    public PopInstruction(Ptr element)
    {
        this.Element = element;
    }


    public PopInstruction(Ptr element, string comment) : base(comment)
    {
        this.Element = element;
    }


    public override string ToString()
    {
        return AppendComment($"\tpop\t{this.Element}");
    }
}