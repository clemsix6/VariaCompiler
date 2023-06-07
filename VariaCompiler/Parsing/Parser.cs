using VariaCompiler.Lexing;
using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Parsing;

public class Parser
{
    private List<Token> _tokens;
    private int         _index;


    public ProgramNode Parse(List<Token> tokens)
    {
        this._index  = 0;
        this._tokens = tokens;

        var functions = new List<Node>();
        while (this._index < this._tokens.Count) functions.Add(ParseFunctionDeclaration());

        return new ProgramNode(functions);
    }


    private Node ParseFunctionDeclaration()
    {
        if (this._tokens[this._index].Type != TokenType.Func) throw new Exception("func keyword expected");
        this._index++;

        if (this._tokens[this._index].Type != TokenType.Identifier) throw new Exception("Function name expected");
        var functionName = this._tokens[this._index++];

        if (this._tokens[this._index].Type != TokenType.LeftParenthesis) throw new Exception("( expected");
        this._index++;

        var parameters = ParseParameters();

        if (this._tokens[this._index].Type != TokenType.RightParenthesis) throw new Exception(") expected");
        this._index++;

        if (this._tokens[this._index].Type != TokenType.BuiltinType)
            throw new Exception("Function return type expected");
        var returnType = this._tokens[this._index++];

        var body = ParseBlock();

        return new FunctionDeclarationNode(returnType, functionName, parameters, body);
    }


    private List<ParameterNode> ParseParameters()
    {
        var parameters = new List<ParameterNode>();

        while (this._tokens[this._index].Type != TokenType.RightParenthesis) {
            if (this._tokens[this._index].Type != TokenType.Identifier) throw new Exception("Parameter type expected");
            var parameterType = this._tokens[this._index++];

            if (this._tokens[this._index].Type != TokenType.Identifier) throw new Exception("Parameter name expected");
            var parameterName = this._tokens[this._index++];

            parameters.Add(new ParameterNode(parameterType, parameterName));

            if (this._tokens[this._index].Type == TokenType.Comma)
                this._index++;
            else if (this._tokens[this._index].Type != TokenType.RightParenthesis)
                throw new Exception(", or ) expected");
        }

        return parameters;
    }


    private BlockNode ParseBlock()
    {
        if (this._tokens[this._index].Type != TokenType.LeftBrace) throw new Exception("{ expected");
        this._index++;

        var statements = new List<Node>();
        while (this._tokens[this._index].Type != TokenType.RightBrace) statements.Add(ParseStatement());

        this._index++;

        return new BlockNode(statements);
    }


    private Node ParseStatement()
    {
        switch (this._tokens[this._index].Type) {
            case TokenType.Return:
            {
                this._index++;
                var expression = ParseExpression();
                if (this._tokens[this._index].Type != TokenType.SemiColon) throw new Exception("; expected");
                this._index++;
                return new ReturnNode(expression);
            }
            case TokenType.Var:
            case TokenType.BuiltinType:
            {
                return ParseVarDeclaration();
            }
            case TokenType.Identifier:
            {
                if (this._tokens[this._index + 1].Type == TokenType.LeftParenthesis) return ParseFunctionCall();
                return ParseAssignment();
            }
            default:
            {
                throw new Exception("Invalid statement");
            }
        }
    }


    private Node ParseVarDeclaration()
    {
        var type = this._tokens[this._index++];
        if (this._tokens[this._index].Type != TokenType.Identifier) throw new Exception("Variable name expected");
        var name = this._tokens[this._index++];

        Node expression = null;
        if (this._tokens[this._index].Type == TokenType.Equals) {
            this._index++;
            expression = ParseExpression();
        }

        if (this._tokens[this._index].Type != TokenType.SemiColon) throw new Exception("; expected");
        if (expression                     == null) throw new Exception("Variable declaration must have an expression");
        this._index++;
        return new AssignmentNode(name, expression, type.Type != TokenType.Var ? type : null);
    }


    private Node ParseAssignment()
    {
        var name = this._tokens[this._index++];

        if (this._tokens[this._index].Type != TokenType.Equals) {
            this._tokens.Insert(this._index, new Token(TokenType.SemiColon, ";"));
            this._tokens.Insert(this._index, new Token(TokenType.Number,    "0"));
            this._tokens.Insert(this._index, new Token(TokenType.Equals,    "="));
        }

        this._index++;
        var expression = ParseExpression();
        if (this._tokens[this._index].Type != TokenType.SemiColon) throw new Exception("; expected");
        this._index++;
        return new AssignmentNode(name, expression);
    }


    private Node ParseBuiltinTypeDeclaration()
    {
        var type = this._tokens[this._index++];
        if (this._tokens[this._index].Type != TokenType.Identifier) throw new Exception("Variable name expected");
        var name = this._tokens[this._index++];

        if (this._tokens[this._index].Type != TokenType.Equals) {
            this._tokens.Insert(this._index, new Token(TokenType.SemiColon, ";"));
            this._tokens.Insert(this._index, new Token(TokenType.Number,    "0"));
            this._tokens.Insert(this._index, new Token(TokenType.Equals,    "="));
        }

        this._index++;

        Node expression;
        if (this._tokens[this._index].Type     == TokenType.Identifier
         && this._tokens[this._index + 1].Type == TokenType.LeftParenthesis)
            expression = ParseFunctionCall();
        else
            expression = ParseExpression();

        if (this._tokens[this._index].Type != TokenType.SemiColon) throw new Exception("; expected");
        this._index++;
        return new AssignmentNode(name, expression);
    }


    private Node ParseExpression()
    {
        var node = ParseTerm();

        while (this._index < this._tokens.Count
            && (this._tokens[this._index].Type == TokenType.Plus
             || this._tokens[this._index].Type == TokenType.Minus)) {
            var token     = this._tokens[this._index++];
            var rightNode = ParseTerm();
            node = new OperatorNode(token, node, rightNode);
        }

        return node;
    }


    private Node ParseTerm()
    {
        var node = ParseFactor();

        while (this._index < this._tokens.Count
            && (this._tokens[this._index].Type == TokenType.Multiply
             || this._tokens[this._index].Type == TokenType.Divide)) {
            var token     = this._tokens[this._index++];
            var rightNode = ParseFactor();
            node = new OperatorNode(token, node, rightNode);
        }

        return node;
    }


    private Node ParseFunctionCall()
    {
        var functionName = this._tokens[this._index++];

        if (this._tokens[this._index].Type != TokenType.LeftParenthesis) throw new Exception("( expected");
        this._index++;

        var arguments = new List<Node>();
        while (this._tokens[this._index].Type != TokenType.RightParenthesis) {
            arguments.Add(ParseExpression());

            if (this._tokens[this._index].Type == TokenType.Comma)
                this._index++;
            else if (this._tokens[this._index].Type != TokenType.RightParenthesis)
                throw new Exception(", or ) expected");
        }

        this._index++;
        return new FunctionCallNode(functionName, arguments);
    }


    private Node ParseFactor()
    {
        switch (this._tokens[this._index].Type) {
            case TokenType.LeftParenthesis:
            {
                this._index++;
                var node = ParseExpression();
                if (this._tokens[this._index].Type != TokenType.RightParenthesis) throw new Exception("Expected )");
                this._index++;
                return node;
            }
            case TokenType.Number:
            {
                var token = this._tokens[this._index++];
                return new NumberNode(token);
            }
            case TokenType.Identifier:
            {
                var token = this._tokens[this._index];
                if (this._tokens[this._index + 1].Type == TokenType.LeftParenthesis) return ParseFunctionCall();
                this._index++;
                return new IdentifierNode(token);
            }
            default: throw new Exception("Number, identifier or ( expected");
        }
    }
}