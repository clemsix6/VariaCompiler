using VariaCompiler.Lexing;


namespace VariaCompiler.Parsing.Nodes;

public class ParameterNode : Node
{
    public Token Type { get; }
    public Token Name { get; }


    public ParameterNode(Token type, Token name)
    {
        this.Type = type;
        this.Name = name;
    }


    public override void Visit(int nest)
    {
        Console.WriteLine($"Parameter: {this.Type.Value} {this.Name.Value}");
    }


    public override List<Node> GetChildren()
    {
        return new List<Node>();
    }
}