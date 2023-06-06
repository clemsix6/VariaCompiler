using VariaCompiler.Lexing;


namespace VariaCompiler.Parsing.Nodes;

public class AssignmentNode : Node
{
    public Token Name       { get; }
    public Node  Expression { get; }


    public AssignmentNode(Token name, Node expression)
    {
        this.Name       = name;
        this.Expression = expression;
    }


    public override void Visit(int nest)
    {
        for (var i = 0; i < nest; i++) Console.Write("\t");
        Console.Write($"Var {this.Name.Value} = ");
        this.Expression.Visit(nest);
    }


    public override List<Node> GetChildren()
    {
        return new List<Node> {this.Expression};
    }
}