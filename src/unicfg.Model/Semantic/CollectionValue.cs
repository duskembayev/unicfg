namespace unicfg.Model.Semantic;

public sealed class CollectionValue : IValue
{
    public CollectionValue(ImmutableArray<IValue> values)
    {
        Values = values;
        SourceRange = values[0].SourceRange.Start..values[^1].SourceRange.End;
    }

    public ImmutableArray<IValue> Values { get; }
    public Range SourceRange { get; }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}