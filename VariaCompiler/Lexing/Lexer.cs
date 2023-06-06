using System.Text.RegularExpressions;


namespace VariaCompiler.Lexing;

public partial class Lexer
{
    private Dictionary<TokenType, Regex> _tokenDefinitions;

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
    [GeneratedRegex("int")]    private static partial Regex RegInt();
    [GeneratedRegex("return")] private static partial Regex RegReturn();


    [GeneratedRegex("\\b[a-zA-Z_]\\w*\\b")]
    private static partial Regex RegIdentifier();


    public Lexer()
    {
        this._tokenDefinitions = new Dictionary<TokenType, Regex>();
        this._tokenDefinitions.Add(TokenType.Number,   RegNumber());
        this._tokenDefinitions.Add(TokenType.Plus,     RegPlus());
        this._tokenDefinitions.Add(TokenType.Minus,    RegMinus());
        this._tokenDefinitions.Add(TokenType.Multiply, RegMultiply());
        this._tokenDefinitions.Add(TokenType.Divide,   RegDivide());
        this._tokenDefinitions.Add(TokenType.Equals,   RegEquals());

        this._tokenDefinitions.Add(TokenType.LeftParenthesis,  RegLeftParenthesis());
        this._tokenDefinitions.Add(TokenType.RightParenthesis, RegRightParenthesis());
        this._tokenDefinitions.Add(TokenType.LeftBrace,        RegLeftBrace());
        this._tokenDefinitions.Add(TokenType.RightBrace,       RegRightBrace());
        this._tokenDefinitions.Add(TokenType.SemiColon,        RegSemicolon());
        this._tokenDefinitions.Add(TokenType.Comma,            RegComma());

        this._tokenDefinitions[TokenType.Func]   = RegFunc();
        this._tokenDefinitions[TokenType.Var]    = RegVar();
        this._tokenDefinitions[TokenType.Int]    = RegInt();
        this._tokenDefinitions[TokenType.Return] = RegReturn();

        this._tokenDefinitions[TokenType.Identifier] = RegIdentifier();
    }


    public List<Token> Tokenize(string source)
    {
        var tokens = new List<Token>();
        while (!string.IsNullOrEmpty(source)) {
            var matchFound = false;
            foreach (var tokenDefinition in this._tokenDefinitions) {
                var match = tokenDefinition.Value.Match(source);
                if (!match.Success || match.Index != 0) continue;
                tokens.Add(new Token(tokenDefinition.Key, match.Value));
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