namespace VariaCompiler.Compiling.Instructions;

public class GlobalInstruction : Instruction
{
    public string Name { get; }


    public GlobalInstruction(string name)
    {
        this.Name = name;
    }


    public override string ToString()
    {
        return $".globl {this.Name}";
    }
}