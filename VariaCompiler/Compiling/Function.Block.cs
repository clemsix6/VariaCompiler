using VariaCompiler.Parsing.Nodes;


namespace VariaCompiler.Compiling;

public partial class Function
{
    private void Visit(BlockNode block)
    {
        foreach (var statement in block.Statements) Visit(statement);
    }
}