namespace unicfg.Model.Semantic;

public class Document : ISemanticNode
{
    private Namespace? _rootNamespace;

    public Namespace RootNamespace
    {
        get => _rootNamespace ?? throw new InvalidOperationException();
        internal set => _rootNamespace = value;
    }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}