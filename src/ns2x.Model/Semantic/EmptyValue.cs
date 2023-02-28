namespace ns2x.Model.Semantic;

public sealed class EmptyValue : IValue
{
    public static readonly EmptyValue Instance = new();

    private EmptyValue()
    {
    }

    public Range SourceRange { get; } = Range.All;

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}