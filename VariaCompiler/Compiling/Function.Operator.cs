using VariaCompiler.Compiling.Instructions;
using VariaCompiler.Compiling.Instructions.Operations;
using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling;

public partial class Function
{
    private Ptr Visit(OperatorNode op, Ptr? assignement)
    {
        var left       = GetOperand(op.Left,  Words.RegisterType.A, out var instruction1, out var size1);
        var right      = GetOperand(op.Right, Words.RegisterType.B, out var instruction2, out var size2);
        var size       = size1 > size2 ? size1 : size2;
        var axRegister = new Register(Words.RegisterType.A, size);

        if (instruction1 != null && !axRegister.Equals(instruction1.Source))
            this._instructions.Add(new MovInstruction(axRegister, instruction1.Source));
        if (instruction2 != null) {
            var target = new Register(Words.RegisterType.B, size);
            this._instructions.Add(new MovInstruction(target, instruction2.Source));
        }

        if (right.Size != left.Size) {
            var bxRegister = new Register(Words.RegisterType.B, size);
            this._instructions.Add(new OperationInstruction(axRegister, bxRegister, op.OperatorToken.Value));
        }
        else {
            this._instructions.Add(new OperationInstruction(axRegister, right, op.OperatorToken.Value));
        }

        if (assignement == null) return left;
        if (!left.Equals(assignement)) this._instructions.Add(new MovInstruction(left, assignement));
        return assignement;
    }


    private Ptr GetOperand(Node node, Words.RegisterType suffix, out MovInstruction? instruction, out int size)
    {
        switch (node) {
            case OperatorNode opNode:
            {
                var result = Visit(opNode, null);
                size        = Words.GetTypeSize(result.GetType());
                instruction = new MovInstruction(new Register(suffix), result);
                return new Register(suffix);
            }
            case NumberNode numNode:
            {
                var number = new Number(numNode.Token.Value);
                size        = Words.GetTypeSize(number.GetType());
                instruction = new MovInstruction(new Register(suffix), number);
                if (suffix != Words.RegisterType.A) return number;
                return new Register(suffix);
            }
            case IdentifierNode idNode1:
            {
                var variable = GetVariable(idNode1.Name.Value);
                if (variable == null) throw new Exception($"Variable \"{idNode1.Name.Value}\" not found");
                size        = Words.GetTypeSize(variable.GetType());
                instruction = new MovInstruction(new Register(suffix), variable);
                if (suffix != Words.RegisterType.A) return variable;
                return new Register(suffix);
            }
            case FunctionCallNode functionCall:
            {
                Visit(functionCall);
                var function = this._functions.Find(x => x.Declaration.Name.Value == functionCall.Name.Value);
                if (function == null) throw new Exception($"Function \"{functionCall.Name.Value}\" not found");
                size        = Words.GetTypeSize(function.Declaration.ReturnType.Value);
                instruction = null;
                return new Register(suffix);
            }
            default:
            {
                throw new Exception("Unsupported operand type");
            }
        }
    }
}