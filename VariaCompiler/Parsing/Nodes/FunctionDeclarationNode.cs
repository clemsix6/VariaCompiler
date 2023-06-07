using VariaCompiler.Lexing;


namespace VariaCompiler.Parsing.Nodes;

public class FunctionDeclarationNode : Node
{
    public Token               ReturnType { get; }
    public Token               Name       { get; }
    public List<ParameterNode> Parameters { get; }
    public BlockNode           Body       { get; }


    public FunctionDeclarationNode(Token returnType, Token name, List<ParameterNode> parameters, BlockNode body)
    {
        this.ReturnType = returnType;
        this.Name       = name;
        this.Parameters = parameters;
        this.Body       = body;
    }


    public override void Visit(int nest)
    {
        Console.WriteLine($"Function declaration: {this.ReturnType.Value} {this.Name.Value}");
        foreach (var parameter in this.Parameters) parameter.Visit(nest + 1);
        this.Body.Visit(nest + 1);
    }


    public override List<Node> GetChildren()
    {
        var children = new List<Node>(this.Parameters);
        children.Add(this.Body);
        return children;
    }
}