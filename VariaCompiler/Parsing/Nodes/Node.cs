namespace VariaCompiler.Parsing.Nodes;

public abstract class Node
{
    public abstract void       Visit(int nest);
    public abstract List<Node> GetChildren();


    private void GetChildren(Node node, List<Node> children)
    {
        children.Add(node);
        foreach (var child in node.GetChildren()) GetChildren(child, children);
    }


    public List<Node> GetChildrenRecursive()
    {
        var children = new List<Node>();
        GetChildren(this, children);
        return children;
    }
}