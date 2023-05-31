namespace VariaCompiler.Lexing;

public enum TokenType
{
    //operators
    Plus,
    Minus,
    Multiply,
    Divide,
    Equals,

    //braces
    LeftParenthesis,
    RightParenthesis,
    LeftBrace,
    RightBrace,
    SemiColon,

    //keywords
    Func,
    Var,
    Int,

    //other
    Identifier,
    Number,
    Return,
    Unknown
}


public class Token
{
    public TokenType Type  { get; }
    public string    Value { get; }


    public Token(TokenType type, string value)
    {
        this.Type  = type;
        this.Value = value;
    }
}