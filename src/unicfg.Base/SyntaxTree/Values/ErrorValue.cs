namespace unicfg.Base.SyntaxTree.Values;

public sealed class ErrorValue : IValue
{
    public static readonly ErrorValue Instance = new();

    private ErrorValue()
    {
    }

    public void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }

    public Range SourceRange { get; } = Range.All;
}