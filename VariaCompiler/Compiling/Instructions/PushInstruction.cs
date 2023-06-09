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


    private void AddAutoComment()
    {
        var element = this.Element.GetIdentifier(true);

        this.Comment = $"Push {element}";
    }


    public override void Build(Function function, List<Instruction> instructions) { }


    public override string ToString()
    {
        if (this.Comment == null) AddAutoComment();
        return AppendComment($"\tpush\t{this.Element}");
    }
}