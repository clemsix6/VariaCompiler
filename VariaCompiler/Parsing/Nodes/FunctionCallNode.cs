using VariaCompiler.Lexing;


namespace VariaCompiler.Parsing.Nodes;

public class FunctionCallNode : Node
{
    public Token      Name      { get; }
    public List<Node> Arguments { get; }


    public FunctionCallNode(Token name, List<Node> arguments)
    {
        this.Name      = name;
        this.Arguments = arguments;
    }


    public override void Visit(int nest)
    {
        for (var i = 0; i < nest; i++) Console.Write("\t");
        Console.WriteLine($"Function call: {this.Name.Value}");
        foreach (var argument in this.Arguments) argument.Visit(nest + 1);
    }


    public override List<Node> GetChildren()
    {
        return this.Arguments;
    }
}