using VariaCompiler.Compiling.Instructions;
using VariaCompiler.Compiling.Instructions.Operations;
using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling;

public partial class Function
{
    private Ptr Visit(OperatorNode op, Ptr? assignement)
    {
        var left      = GetOperand(op.Left,  Words.registers[Words.RegisterType.A],         null);
        var right     = GetOperand(op.Right, new Register(Words.RegisterType.B, left.Size), left);
        var operation = new OperationInstruction(left, right, op.OperatorToken.Value);
        this._instructions.Add(operation);

        if (assignement == null) return left;
        if (!left.Equals(assignement)) this._instructions.Add(new MovInstruction(assignement, left));
        return assignement;
    }


    private Ptr GetOperand(Node node, Register? destination, Ptr? left)
    {
        switch (node) {
            case OperatorNode opNode:
            {
                Variable? tempVariable         = null;
                if (left != null) tempVariable = createTempVariable(left);

                var result = Visit(opNode, destination);
                if (tempVariable != null) {
                    var mov = new MovInstruction(new Register(Words.RegisterType.A, tempVariable.Size), tempVariable);
                    this._instructions.Add(mov);
                }

                return result;
            }
            case NumberNode numNode:
            {
                var number = new Number(numNode.Token.Value);

                if (left == null || destination?.Type == Words.RegisterType.A) {
                    destination = new Register(Words.RegisterType.A, number.Size);
                    var mov = new MovInstruction(destination, number);
                    this._instructions.Add(mov);
                    return destination;
                }

                return number;
            }
            case IdentifierNode idNode1:
            {
                var variable = GetVariable(idNode1.Name.Value);
                if (variable == null) throw new Exception($"Variable \"{idNode1.Name.Value}\" not found");
                if (destination != null && destination.Type == Words.RegisterType.A) {
                    destination = new Register(Words.RegisterType.A, variable.Size);
                    var mov = new MovInstruction(destination, variable);
                    this._instructions.Add(mov);
                    return destination;
                }

                return variable;
            }
            case FunctionCallNode functionCall:
            {
                Variable? tempVariable         = null;
                if (left != null) tempVariable = createTempVariable(left);
                Visit(functionCall);
                if (tempVariable != null) {
                    var size = GetFunctionReturnSize(functionCall.Name.Value);
                    if (size == null) throw new Exception($"Function \"{functionCall.Name.Value}\" not found");
                    var mov = new MovInstruction(
                        new Register(Words.RegisterType.B, size.Value),
                        new Register(Words.RegisterType.A, size.Value)
                    );
                    this._instructions.Add(mov);
                    mov = new MovInstruction(new Register(Words.RegisterType.A, tempVariable.Size), tempVariable);
                    this._instructions.Add(mov);
                }

                return destination;
            }
            default:
            {
                throw new Exception("Unsupported operand type");
            }
        }
    }
}