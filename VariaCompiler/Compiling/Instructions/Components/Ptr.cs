namespace VariaCompiler.Compiling.Instructions;

public abstract class Ptr
{
    public int Size { get; protected set; }

    public bool IsAssignable => this is Variable || this is Register;

    public abstract string GetType();

    public abstract string GetIdentifier(bool format = false);


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