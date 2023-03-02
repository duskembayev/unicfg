using unicfg.Model.Primitives;

namespace unicfg.Model.Semantic;

public sealed class RefValue : IValue
{
    public RefValue(Range sourceRange, PropertyRef property)
    {
        SourceRange = sourceRange;
        Property = property;
    }

    public Range SourceRange { get; }
    public PropertyRef Property { get; }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}