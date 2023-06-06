namespace VariaCompiler.Compiling;

public class Variable
{
    public string Name  { get; }
    public int    Stack { get; }
    public int    Size  { get; }


    public Variable(string name, int stack, int size)
    {
        this.Name  = name;
        this.Stack = stack;
        this.Size  = size;
    }
}