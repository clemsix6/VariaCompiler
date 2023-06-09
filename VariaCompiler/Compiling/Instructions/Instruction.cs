namespace VariaCompiler.Compiling.Instructions;

public abstract class Instruction
{
    public string? Comment { get; set; }


    public Instruction()
    {
        this.Comment = null;
    }


    public Instruction(string comment)
    {
        this.Comment = comment;
    }


    protected string AppendComment(string line)
    {
        if (this.Comment == null) return line;
        var len = line.Where(x => x != '\n').Select(x => x == '\t' ? 8 : 1).Sum();
        return $"{line}{new string(' ', 60 - len)}\t# {this.Comment}";
    }


    public abstract void Build(Function function, List<Instruction> instructions);
}