using VariaCompiler.Compiling.Instructions;
using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling;

public partial class Function
{
    private void Visit(ReturnNode returnNode)
    {
        var expression = returnNode.Expression;

        switch (expression) {
            case NumberNode:
            {
                this._instructions.Add(
                    new MovInstruction(
                        new Register(
                            Words.RegisterType.A,
                            Words.GetTypeSize(this.Declaration.ReturnType.Value)
                        ),
                        new Number((returnNode.Expression as NumberNode)!.Token.Value)
                    )
                );
                break;
            }
            case IdentifierNode identifier:
            {
                var variable = GetVariable(identifier.Name.Value);
                if (variable == null) throw new Exception($"Variable \"{variable}\" not found");
                this._instructions.Add(
                    new MovInstruction(
                        new Register(
                            Words.RegisterType.A,
                            Words.GetTypeSize(this.Declaration.ReturnType.Value)
                        ),
                        variable,
                        $"Copy value of \"{identifier.Name.Value}\" to ?ax for return"
                    )
                );
                break;
            }
            case OperatorNode opNode:
            {
                var register = new Register(Words.RegisterType.A, Words.GetTypeSize(this.Declaration.ReturnType.Value));
                Visit(opNode, register);
                break;
            }
            case FunctionCallNode functionCall:
            {
                Visit(functionCall);
                break;
            }
        }

        this._instructions.Add(new PopInstruction(new Register(Words.RegisterType.BP)));
        this._instructions.Add(new RetInstruction());
    }
}