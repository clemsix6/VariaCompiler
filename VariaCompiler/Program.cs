using VariaCompiler.Compiling;
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
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Lexing...");
        var lexer  = new Lexer();
        var tokens = lexer.Tokenize(content);
        foreach (var token in tokens) Console.WriteLine("Token: \"" + token.Value + "\"\t(" + token.Type + ")");
        return tokens;
    }


    private static ProgramNode Parse(List<Token> tokens)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n\n\nParsing...");
        var parser = new Parser();
        var node   = parser.Parse(tokens);
        node.Visit(0);
        return node;
    }


    private static string Detail(ProgramNode node)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\n\n\nDetailing...");
        var detailer = new Detailer();
        var details  = detailer.Compile(node);
        Console.WriteLine(details);
        return details;
    }
}