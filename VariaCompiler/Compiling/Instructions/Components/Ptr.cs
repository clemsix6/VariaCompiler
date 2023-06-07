namespace VariaCompiler.Compiling.Instructions;

public abstract class Ptr
{
    public int Size { get; protected set; }

    public abstract string GetType();


    public override bool Equals(object? obj)
    {
        if (this is Variable currentVariable && obj is Variable parameterVariable)
            return currentVariable.Equals(parameterVariable);
        if (this is Register currentRegister && obj is Register parameterRegister)
            return currentRegister.Equals(parameterRegister);
        if (this is Number currentNumber && obj is Number parameterNumber) return currentNumber.Equals(parameterNumber);
        return false;
    }
}