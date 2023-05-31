using VariaCompiler.Lexing;


namespace VariaCompiler.Parsing.Nodes;

public class IdentifierNode : Node
{
    public Token Name { get; }

    public IdentifierNode(Token name)
    {
        this.Name = name;
    }

    public override void Visit(int nest)
    {
        for (var i = 0; i < nest; i++) Console.Write("\t");
        Console.WriteLine(this.Name.Value);
    }
}
