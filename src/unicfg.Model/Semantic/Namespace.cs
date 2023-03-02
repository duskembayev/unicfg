using unicfg.Model.Primitives;

namespace unicfg.Model.Semantic;

public sealed class Namespace : SemanticNodeWithName
{
    public Namespace(StringRef name, Document document, SemanticNodeWithName? parent)
        : base(name, document, parent)
    {
    }

    public ImmutableArray<Namespace> Namespaces { get; internal set; }
    public ImmutableArray<Property> Properties { get; internal set; }
    public ImmutableArray<Attribute> Attributes { get; internal set; }

    public override void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}