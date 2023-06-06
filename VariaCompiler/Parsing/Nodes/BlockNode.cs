namespace VariaCompiler.Parsing.Nodes;

public class BlockNode : Node
{
    public List<Node> Statements { get; }


    public BlockNode(List<Node> statements)
    {
        this.Statements = statements;
    }


    public override void Visit(int nest)
    {
        for (var i = 0; i < nest; i++) Console.Write("\t");
        Console.WriteLine("Block start");
        foreach (var statement in this.Statements) statement.Visit(nest + 1);
        for (var i = 0; i < nest; i++) Console.Write("\t");
        Console.WriteLine("Block end");
    }


    public override List<Node> GetChildren()
    {
        return this.Statements;
    }
}