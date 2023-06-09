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


    private void AddAutoComment()
    {
        var element = this.Element.GetIdentifier(true);

        this.Comment = $"Pop {element}";
    }


    public override void Build(Function function, List<Instruction> instructions) { }


    public override string ToString()
    {
        if (this.Comment == null) AddAutoComment();
        return AppendComment($"\tpop\t{this.Element}");
    }
}