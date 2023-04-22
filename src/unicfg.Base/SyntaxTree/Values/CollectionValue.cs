namespace unicfg.Base.SyntaxTree.Values;

public sealed class CollectionValue : IValue
{
    public CollectionValue(ImmutableArray<IValue> values)
    {
        Values = values;
        SourceRange = values[0].SourceRange.Start..values[^1].SourceRange.End;
    }

    public ImmutableArray<IValue> Values { get; }
    public Range SourceRange { get; }

    public T Accept<T>(IElementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
