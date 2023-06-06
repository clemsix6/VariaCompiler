using VariaCompiler.Lexing;


namespace VariaCompiler.Parsing.Nodes;

public class FunctionDeclarationNode : Node
{
    public Token     ReturnType { get; }
    public Token     Name       { get; }
    public BlockNode Body       { get; }


    public FunctionDeclarationNode(Token returnType, Token name, BlockNode body)
    {
        this.ReturnType = returnType;
        this.Name       = name;
        this.Body       = body;
    }


    public override void Visit(int nest)
    {
        Console.WriteLine($"Function declaration: {this.ReturnType.Value} {this.Name.Value}");
        this.Body.Visit(nest + 1);
    }


    public override List<Node> GetChildren()
    {
        return new List<Node> {this.Body};
    }
}