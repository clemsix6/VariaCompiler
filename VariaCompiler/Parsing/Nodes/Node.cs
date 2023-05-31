namespace VariaCompiler.Parsing.Nodes;

public abstract class Node
{
    public abstract void Visit(int nest);
}