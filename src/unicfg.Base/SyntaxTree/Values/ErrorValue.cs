namespace unicfg.Base.SyntaxTree.Values;

public sealed class ErrorValue : IValue
{
    public static readonly ErrorValue Instance = new();

    private ErrorValue()
    {
    }

    public Range SourceRange { get; } = Range.All;

    public T Accept<T>(IElementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}