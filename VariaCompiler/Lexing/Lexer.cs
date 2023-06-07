using System.Text.RegularExpressions;


namespace VariaCompiler.Lexing;

public partial class Lexer
{
    private List<(TokenType, Regex)> _tokenDefinitions;

    [GeneratedRegex("\\d+")] private static partial Regex RegNumber();
    [GeneratedRegex("\\+")]  private static partial Regex RegPlus();
    [GeneratedRegex("\\-")]  private static partial Regex RegMinus();
    [GeneratedRegex("\\*")]  private static partial Regex RegMultiply();
    [GeneratedRegex("\\/")]  private static partial Regex RegDivide();
    [GeneratedRegex("\\=")]  private static partial Regex RegEquals();

    [GeneratedRegex("\\(")] private static partial Regex RegLeftParenthesis();
    [GeneratedRegex("\\)")] private static partial Regex RegRightParenthesis();
    [GeneratedRegex("\\{")] private static partial Regex RegLeftBrace();
    [GeneratedRegex("\\}")] private static partial Regex RegRightBrace();
    [GeneratedRegex("\\;")] private static partial Regex RegSemicolon();
    [GeneratedRegex("\\,")] private static partial Regex RegComma();

    [GeneratedRegex("func")]   private static partial Regex RegFunc();
    [GeneratedRegex("var")]    private static partial Regex RegVar();
    [GeneratedRegex("return")] private static partial Regex RegReturn();

    [GeneratedRegex("byte")]   private static partial Regex RegByte();
    [GeneratedRegex("short")]  private static partial Regex RegShort();
    [GeneratedRegex("int")]    private static partial Regex RegInt();
    [GeneratedRegex("long")]   private static partial Regex RegLong();
    [GeneratedRegex("float")]  private static partial Regex RegFloat();
    [GeneratedRegex("double")] private static partial Regex RegDouble();
    [GeneratedRegex("void")]   private static partial Regex RegVoid();


    [GeneratedRegex("\\b[a-zA-Z_]\\w*\\b")]
    private static partial Regex RegIdentifier();


    public Lexer()
    {
        this._tokenDefinitions = new List<(TokenType, Regex)>();

        this._tokenDefinitions.Add((TokenType.Number, RegNumber()));
        this._tokenDefinitions.Add((TokenType.Plus, RegPlus()));
        this._tokenDefinitions.Add((TokenType.Minus, RegMinus()));
        this._tokenDefinitions.Add((TokenType.Multiply, RegMultiply()));
        this._tokenDefinitions.Add((TokenType.Divide, RegDivide()));
        this._tokenDefinitions.Add((TokenType.Equals, RegEquals()));

        this._tokenDefinitions.Add((TokenType.LeftParenthesis, RegLeftParenthesis()));
        this._tokenDefinitions.Add((TokenType.RightParenthesis, RegRightParenthesis()));
        this._tokenDefinitions.Add((TokenType.LeftBrace, RegLeftBrace()));
        this._tokenDefinitions.Add((TokenType.RightBrace, RegRightBrace()));
        this._tokenDefinitions.Add((TokenType.SemiColon, RegSemicolon()));
        this._tokenDefinitions.Add((TokenType.Comma, RegComma()));

        this._tokenDefinitions.Add((TokenType.Func, RegFunc()));
        this._tokenDefinitions.Add((TokenType.Var, RegVar()));
        this._tokenDefinitions.Add((TokenType.Return, RegReturn()));

        this._tokenDefinitions.Add((TokenType.BuiltinType, RegInt()));
        this._tokenDefinitions.Add((TokenType.BuiltinType, RegByte()));
        this._tokenDefinitions.Add((TokenType.BuiltinType, RegShort()));
        this._tokenDefinitions.Add((TokenType.BuiltinType, RegLong()));
        this._tokenDefinitions.Add((TokenType.BuiltinType, RegFloat()));
        this._tokenDefinitions.Add((TokenType.BuiltinType, RegDouble()));
        this._tokenDefinitions.Add((TokenType.BuiltinType, RegVoid()));

        this._tokenDefinitions.Add((TokenType.Identifier, RegIdentifier()));
    }


    public List<Token> Tokenize(string source)
    {
        var tokens = new List<Token>();
        while (!string.IsNullOrEmpty(source)) {
            var matchFound = false;
            foreach (var (tokenType, regex) in this._tokenDefinitions) {
                var match = regex.Match(source);
                if (!match.Success || match.Index != 0) continue;
                tokens.Add(new Token(tokenType, match.Value));
                source     = TrimStart(source, match.Length);
                matchFound = true;
                break;
            }

            if (matchFound) continue;
            if (!char.IsWhiteSpace(source[0])) tokens.Add(new Token(TokenType.Unknown, source[0].ToString()));
            source = TrimStart(source, 1);
        }

        return tokens;
    }


    private string TrimStart(string s, int length)
    {
        return s.Substring(length, s.Length - length);
    }
}