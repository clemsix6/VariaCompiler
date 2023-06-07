namespace VariaCompiler.Compiling.Instructions;

public class Variable : Ptr
{
    public string Name  { get; }
    public string Type  { get; }
    public int    Stack { get; }


    public Variable(string name, string type, int stack, int size)
    {
        this.Name  = name;
        this.Type  = type;
        this.Stack = stack;
        this.Size  = size;
    }


    public override string GetType()
    {
        return this.Type;
    }


    public override string ToString()
    {
        return $"{Words.GetWordType(this.Size)} ptr [rbp-{this.Stack}]";
    }


    public override bool Equals(object? obj)
    {
        if (obj is Variable variable)
            return this.Name  == variable.Name
                && this.Type  == variable.Type
                && this.Stack == variable.Stack
                && this.Size  == variable.Size;
        return false;
    }


    protected bool Equals(Variable other)
    {
        return this.Name  == other.Name
            && this.Type  == other.Type
            && this.Stack == other.Stack;
    }


    public override int GetHashCode()
    {
        return HashCode.Combine(this.Name, this.Type, this.Stack);
    }
}