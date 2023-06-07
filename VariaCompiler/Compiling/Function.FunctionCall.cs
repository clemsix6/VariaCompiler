using VariaCompiler.Compiling.Instructions;
using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling;

public partial class Function
{
    private void Visit(FunctionCallNode functionCall)
    {
        var args     = new[] {Words.RegisterType.DI, Words.RegisterType.SI, Words.RegisterType.D, Words.RegisterType.C};
        var function = this._functions.Find(x => x.Declaration.Name.Value == functionCall.Name.Value);

        if (function == null) throw new Exception($"Function \"{functionCall.Name.Value}\" not found");
        HandleFunctionCallArguments(functionCall, function.Declaration, args);
        HandleArgumentTypes(functionCall, function.Declaration, args);

        this._instructions.Add(new CallInstruction(functionCall.Name.Value));
    }


    private void HandleFunctionCallArguments(
        FunctionCallNode                  functionCall,
        FunctionDeclarationNode           functionDeclaration,
        IReadOnlyList<Words.RegisterType> args
    )
    {
        for (var index = 0; index < functionCall.Arguments.Count; index++) {
            var argument = functionCall.Arguments[index];
            if (argument is not FunctionCallNode functionCallNode) continue;
            Visit(functionCallNode);
            this._instructions.Add(
                new MovInstruction(
                    new Register(
                        args[index],
                        Words.GetTypeSize(functionDeclaration.Parameters[index].Type.Value)
                    ),
                    new Register(Words.RegisterType.A)
                )
            );
        }
    }


    private void HandleArgumentTypes(
        FunctionCallNode                  functionCall,
        FunctionDeclarationNode           functionDeclaration,
        IReadOnlyList<Words.RegisterType> args
    )
    {
        for (var index = 0; index < functionCall.Arguments.Count; index++) {
            var argument = functionCall.Arguments[index];

            switch (argument) {
                case NumberNode numNode:
                {
                    var result = new Number(numNode.Token.Value);
                    this._instructions.Add(
                        new MovInstruction(
                            new Register(
                                args[index],
                                Words.GetTypeSize(functionDeclaration.Parameters[index].Type.Value)
                            ),
                            result
                        )
                    );
                    break;
                }
                case IdentifierNode idNode:
                {
                    var variable = GetVariable(idNode.Name.Value);
                    if (variable == null) throw new Exception($"Variable \"{idNode.Name.Value}\" not found");
                    this._instructions.Add(
                        new MovInstruction(
                            new Register(
                                args[index],
                                Words.GetTypeSize(functionDeclaration.Parameters[index].Type.Value)
                            ),
                            variable
                        )
                    );
                    break;
                }
                case OperatorNode operatorNode:
                {
                    var register = new Register(
                        Words.RegisterType.A,
                        Words.GetTypeSize(functionDeclaration.Parameters[index].Type.Value)
                    );
                    Visit(operatorNode, register);
                    this._instructions.Add(
                        new MovInstruction(
                            new Register(
                                args[index],
                                Words.GetTypeSize(functionDeclaration.Parameters[index].Type.Value)
                            ),
                            register
                        )
                    );
                    break;
                }
            }
        }
    }
}