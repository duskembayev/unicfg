namespace unicfg.Base.SyntaxTree.Values;

public sealed class EmptyValue : IValue
{
    public static readonly EmptyValue Instance = new();

    private EmptyValue()
    {
    }

    public Range SourceRange { get; } = Range.All;

    public T Accept<T>(IElementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
