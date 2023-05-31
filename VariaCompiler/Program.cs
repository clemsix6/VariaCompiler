using VariaCompiler.Detailing;
using VariaCompiler.Lexing;
using VariaCompiler.Parsing;
using VariaCompiler.Parsing.Nodes;


internal class Program
{
    public static void Main()
    {
        var content = File.ReadAllText("main.vr");

        var tokens = Lex(content);
        var node   = Parse(tokens);
        var detail = Detail(node);
    }


    private static List<Token> Lex(string content)
    {
        Console.WriteLine("Lexing...");
        var lexer  = new Lexer();
        var tokens = lexer.Tokenize(content);
        foreach (var token in tokens) Console.WriteLine("Token: \"" + token.Value + "\"\t(" + token.Type + ")");
        return tokens;
    }


    private static Node Parse(List<Token> tokens)
    {
        Console.WriteLine("\n\nParsing...");
        var parser = new Parser();
        var node   = parser.Parse(tokens);
        node.Visit(0);
        return node;
    }


    private static string Detail(Node node)
    {
        Console.WriteLine("\n\nDetailing...");
        var detailer = new Detailer();
        var details  = detailer.Compile(node);
        Console.WriteLine("\n\nDetails:\n" + details);
        return details;
    }
}