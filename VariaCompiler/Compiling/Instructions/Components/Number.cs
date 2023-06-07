namespace VariaCompiler.Compiling.Instructions;

public class Number : Ptr
{
    public string number { get; }


    public Number(string number)
    {
        if (!long.TryParse(number, out var n)) throw new Exception($"Invalid number: {number}");
        this.number = number;
        this.Size   = GetTypeSize();
    }


    public Number(long number)
    {
        this.number = number.ToString();
        this.Size   = GetTypeSize();
    }


    public override string GetType()
    {
        switch (this.Size) {
            case 1: return "byte";
            case 2: return "short";
            case 4: return "int";
            case 8: return "long";
        }

        throw new Exception($"Invalid number size: {this.Size}");
    }


    private int GetTypeSize()
    {
        switch (long.Parse(this.number)) {
            case < short.MaxValue: return 2;
            case < int.MaxValue:   return 4;
            default:               return 8;
        }
    }


    public override string ToString()
    {
        return this.number;
    }


    public override bool Equals(object? obj)
    {
        if (obj is Number number) return this.number == number.number;
        return false;
    }


    protected bool Equals(Number other)
    {
        return this.number == other.number;
    }


    public override int GetHashCode()
    {
        return this.number.GetHashCode();
    }
}