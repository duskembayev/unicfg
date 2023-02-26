namespace ns2x.Model.Semantic;

public class Document : ISemanticNode
{
    public Document(Namespace rootNamespace)
    {
        RootNamespace = rootNamespace;
    }

    public Namespace RootNamespace { get; }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}