using System.Text;
using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling;

public class Detailer
{
    private List<Function> functions;
    private Function       currentFunction;


    public Detailer()
    {
        this.functions = new List<Function>();
    }


    public string Compile(ProgramNode node)
    {
        var builder = new StringBuilder();

        Visit(node);
        foreach (var function in this.functions) builder.AppendLine(function.ToString());
        return builder.ToString();
    }


    private void Visit(ProgramNode node)
    {
        foreach (var nodeFunction in node.Functions) {
            var function = new Function((FunctionDeclarationNode) nodeFunction, this.functions);
            this.functions.Add(function);
            this.currentFunction = function;
        }
    }
}