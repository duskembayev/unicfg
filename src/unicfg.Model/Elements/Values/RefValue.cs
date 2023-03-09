namespace unicfg.Model.Elements.Values;

public sealed class RefValue : IValue
{
    public RefValue(Range sourceRange, PropertyRef property)
    {
        SourceRange = sourceRange;
        Property = property;
    }

    public Range SourceRange { get; }
    public PropertyRef Property { get; }

    public void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }
}