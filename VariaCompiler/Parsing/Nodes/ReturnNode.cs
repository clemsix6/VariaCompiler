namespace VariaCompiler.Parsing.Nodes;

public class ReturnNode : Node
{
    public Node Expression { get; }


    public ReturnNode(Node expression)
    {
        this.Expression = expression;
    }


    public override void Visit(int nest)
    {
        for (var i = 0; i < nest; i++) Console.Write("\t");
        Console.WriteLine("Return");
        this.Expression.Visit(nest);
    }


    public override List<Node> GetChildren()
    {
        return new List<Node> {this.Expression};
    }
}