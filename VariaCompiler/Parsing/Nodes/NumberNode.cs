using VariaCompiler.Lexing;


namespace VariaCompiler.Parsing.Nodes;

public class NumberNode : Node
{
    public Token Token { get; }


    public NumberNode(Token token)
    {
        if (token.Type != TokenType.Number) throw new ArgumentException("Number token expected");
        this.Token = token;
    }


    public override void Visit(int nest)
    {
        for (var i = 0; i < nest; i++) Console.Write("\t");
        Console.WriteLine($"Number: {this.Token.Value}");
    }
}