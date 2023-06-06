namespace VariaCompiler.Parsing.Nodes;

public class ProgramNode : Node
{
    public List<Node> Functions { get; }


    public ProgramNode(List<Node> functions)
    {
        this.Functions = functions;
    }


    public override void Visit(int nest)
    {
        for (var i = 0; i < nest; i++) Console.Write("\t");
        Console.WriteLine("Program start");
        foreach (var function in this.Functions) function.Visit(nest + 1);
        for (var i = 0; i < nest; i++) Console.Write("\t");
        Console.WriteLine("Program end");
    }


    public override List<Node> GetChildren()
    {
        return this.Functions;
    }
}