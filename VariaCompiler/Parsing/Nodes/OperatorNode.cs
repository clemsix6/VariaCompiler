using VariaCompiler.Lexing;


namespace VariaCompiler.Parsing.Nodes;

public class OperatorNode : Node
{
    public Token OperatorToken { get; private set; }
    public Node  Left          { get; private set; }
    public Node  Right         { get; private set; }


    public OperatorNode(Token operatorToken, Node left, Node right)
    {
        this.OperatorToken = operatorToken;
        this.Left          = left;
        this.Right         = right;
    }


    public override void Visit(int nest)
    {
        for (var i = 0; i < nest; i++) Console.Write("\t");
        Console.WriteLine($"Operator: {this.OperatorToken.Value}");
        this.Left.Visit(nest  + 1);
        this.Right.Visit(nest + 1);
    }


    public override List<Node> GetChildren()
    {
        return new List<Node> {this.Left, this.Right};
    }
}