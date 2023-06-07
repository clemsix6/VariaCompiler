namespace VariaCompiler.Compiling.Instructions;

public class MovInstruction : Instruction
{
    public Ptr Destination { get; }
    public Ptr Source      { get; }


    public MovInstruction(Ptr destination, Ptr source)
    {
        this.Destination = destination;
        this.Source      = source;
    }


    public MovInstruction(Ptr destination, Ptr source, string comment) : base(comment)
    {
        this.Destination = destination;
        this.Source      = source;
    }


    public override string ToString()
    {
        var command = string.Empty;
        if (this.Source.Size == this.Destination.Size || this.Source is Number) {
            command = "mov";
        }
        else if (this.Source is Variable variable) {
            var currentWord = Words.GetWordType(variable.Size);
            var targetWord  = Words.GetWordType(this.Destination.Size);
            return AppendComment(
                $"\tmov\t{this.Destination}, {this.Source.ToString().Replace(currentWord, targetWord)}"
            );
        }
        else if (this.Source is Register register) {
            var newRegister = Words.GetRegisterName(register.Type, this.Destination.Size);
            return AppendComment(
                $"\tmov\t{this.Destination}, {newRegister}"
            );
        }

        return AppendComment(
            $"\tmov\t{this.Destination}, {this.Source}"
        );
    }
}