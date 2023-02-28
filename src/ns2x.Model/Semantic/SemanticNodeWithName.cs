namespace ns2x.Model.Semantic;

public abstract class SemanticNodeWithName : ISemanticNode
{
    protected SemanticNodeWithName(StringRef name, Document document, SemanticNodeWithName? parent)
    {
        Name = name;
        Document = document;
        Parent = parent;
    }

    public StringRef Name { get; }
    public Document Document { get; }
    public SemanticNodeWithName? Parent { get; }

    public abstract void Accept(ISemanticNodeVisitor visitor);
}