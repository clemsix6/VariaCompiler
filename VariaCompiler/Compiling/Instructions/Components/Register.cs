namespace VariaCompiler.Compiling.Instructions;

public class Register : Ptr
{
    public Words.RegisterType Type { get; }


    public Register(Words.RegisterType type, int size)
    {
        this.Type = type;
        this.Size = size;
    }


    public Register(Words.RegisterType type)
    {
        this.Type = type;
        this.Size = 8;
    }


    public override string GetType()
    {
        switch (this.Size) {
            case 1: return "byte";
            case 2: return "short";
            case 4: return "int";
            case 8: return "long";
        }

        throw new Exception($"Invalid register size: {this.Size}");
    }


    public override string ToString()
    {
        return Words.GetRegisterName(this.Type, this.Size);
    }


    public override bool Equals(object? obj)
    {
        if (obj is Register register) return this.Type == register.Type;
        return false;
    }


    protected bool Equals(Register other)
    {
        return this.Type == other.Type;
    }


    public override int GetHashCode()
    {
        return (int) this.Type;
    }
}