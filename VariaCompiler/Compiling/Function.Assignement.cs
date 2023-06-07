using VariaCompiler.Compiling.Instructions;
using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling;

public partial class Function
{
    private void Visit(AssignmentNode assignment)
    {
        if (assignment.Type == null)
            ImplicitType(assignment);
        else
            ExplicitType(assignment);
    }


    private void ImplicitType(AssignmentNode assignment)
    {
        switch (assignment.Expression) {
            case NumberNode numNode:
            {
                var value = new Number(numNode.Token.Value);
                CreateVariable(assignment.Name.Value, value);
                break;
            }
            case IdentifierNode idNode:
            {
                var variable = GetVariable(idNode.Name.Value);
                if (variable == null) throw new Exception($"Variable \"{variable}\" not found");
                CreateVariable(assignment.Name.Value, variable);
                break;
            }
            case OperatorNode opNode:
            {
                var result = Visit(opNode, null);
                CreateVariable(assignment.Name.Value, result);
                break;
            }
            case FunctionCallNode functionCall:
            {
                var function = this._functions.Find(x => x.Declaration.Name == functionCall.Name);
                if (function == null) throw new Exception($"Function \"{functionCall.Name.Value}\" not found");
                Visit(functionCall);
                var register = new Register(
                    Words.RegisterType.A,
                    Words.GetTypeSize(function.Declaration.ReturnType.Value)
                );
                CreateVariable(assignment.Name.Value, register);
                break;
            }
        }
    }


    private void ExplicitType(AssignmentNode assignment)
    {
        if (assignment.Type == null) throw new Exception("Type is null");

        switch (assignment.Expression) {
            case NumberNode numNode:
            {
                var value = new Number(numNode.Token.Value);
                CreateVariable(assignment.Name.Value, assignment.Type.Value, value);
                break;
            }
            case IdentifierNode idNode:
            {
                var variable = GetVariable(idNode.Name.Value);
                if (variable == null) throw new Exception($"Variable \"{variable}\" not found");
                CreateVariable(assignment.Name.Value, assignment.Type.Value, variable);
                break;
            }
            case OperatorNode opNode:
            {
                var result = Visit(opNode, null);
                CreateVariable(assignment.Name.Value, assignment.Type.Value, result);
                break;
            }
            case FunctionCallNode functionCall:
            {
                var function = this._functions.Find(x => x.Declaration.Name.Value == functionCall.Name.Value);
                if (function == null) throw new Exception($"Function \"{functionCall.Name.Value}\" not found");
                Visit(functionCall);
                var register = new Register(
                    Words.RegisterType.A,
                    Words.GetTypeSize(function.Declaration.ReturnType.Value)
                );
                CreateVariable(assignment.Name.Value, assignment.Type.Value, register);
                break;
            }
        }
    }
}