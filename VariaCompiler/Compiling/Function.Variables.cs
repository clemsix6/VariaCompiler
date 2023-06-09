using VariaCompiler.Compiling.Instructions;


namespace VariaCompiler.Compiling;

public partial class Function
{
    private Variable? CreateVariable(string name, Ptr value)
    {
        var type     = value.GetType();
        var typeSize = value.Size;

        var stack = this._variables.Any() ? this._variables.Max(x => x.Stack) + typeSize : typeSize;
        if (this._variables.Any(x => x.Name == name)) return null;
        var variable = new Variable(name, type, stack, typeSize);
        this._variables.Add(variable);
        this._instructions.Add(
            new MovInstruction(
                variable,
                value,
                $"Create variable {variable.GetIdentifier(true)} with value {value.GetIdentifier(true)}"
            )
        );
        return variable;
    }


    private Variable? CreateVariable(string name, string type, Ptr value)
    {
        var typeSize = Words.GetTypeSize(type);

        var stack = this._variables.Any() ? this._variables.Max(x => x.Stack) + typeSize : typeSize;
        if (this._variables.Any(x => x.Name == name)) return null;
        var variable = new Variable(name, type, stack, typeSize);
        this._variables.Add(variable);
        this._instructions.Add(
            new MovInstruction(
                variable,
                value,
                $"Create variable {variable.GetIdentifier(true)} with value {value.GetIdentifier(true)}"
            )
        );
        return variable;
    }


    private Variable? createTempVariable(Ptr value)
    {
        var name     = $"__temp_{this._tempCounter++}";
        var type     = value.GetType();
        var typeSize = value.Size;

        var stack = this._variables.Any() ? this._variables.Max(x => x.Stack) + typeSize : typeSize;
        if (this._variables.Any(x => x.Name == name)) return null;
        var variable = new Variable(name, type, stack, typeSize);
        this._variables.Add(variable);
        this._instructions.Add(
            new MovInstruction(
                variable,
                value,
                $"Create variable {variable.GetIdentifier(true)} with value {value.GetIdentifier(true)}"
            )
        );
        return variable;
    }


    private Variable? createTempVariable(string type)
    {
        var name     = $"__temp_{this._tempCounter++}";
        var typeSize = Words.GetTypeSize(type);

        var stack = this._variables.Any() ? this._variables.Max(x => x.Stack) + typeSize : typeSize;
        if (this._variables.Any(x => x.Name == name)) return null;
        var variable = new Variable(name, type, stack, typeSize);
        this._variables.Add(variable);
        var value = new Number(0);
        this._instructions.Add(
            new MovInstruction(
                variable,
                value,
                $"Create variable {variable.GetIdentifier(true)} with value {value.GetIdentifier(true)}"
            )
        );
        return variable;
    }


    private Variable? GetVariable(string name)
    {
        return this._variables.Find(x => x.Name == name);
    }


    private string? GetFunctionReturnType(string functionName)
    {
        return this._functions.Find(x => x.Declaration.Name.Value == functionName)?.Declaration.ReturnType.Value;
    }


    private int? GetFunctionReturnSize(string functionName)
    {
        var type = GetFunctionReturnType(functionName);
        if (type == null) return null;
        return Words.GetTypeSize(type);
    }
}