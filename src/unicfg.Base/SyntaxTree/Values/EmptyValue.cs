namespace unicfg.Base.SyntaxTree.Values;

public sealed class EmptyValue : IValue
{
    public static readonly EmptyValue Instance = new();

    private EmptyValue()
    {
    }

    public Range SourceRange { get; } = Range.All;

    public void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }
}