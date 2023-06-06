using VariaCompiler.Lexing;


namespace VariaCompiler.Parsing.Nodes;

public class IntNode : Node
{
    public Token Keyword { get; }
    public Token Name    { get; }


    public IntNode(Token keyword, Token name)
    {
        this.Keyword = keyword;
        this.Name    = name;
    }


    public override void Visit(int nest)
    {
        for (var i = 0; i < nest; i++) Console.Write("\t");
        Console.WriteLine($"Var {this.Keyword.Value} {this.Name.Value}");
    }


    public override List<Node> GetChildren()
    {
        return new List<Node>();
    }
}