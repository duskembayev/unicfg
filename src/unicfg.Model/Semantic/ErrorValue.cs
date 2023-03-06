namespace unicfg.Model.Semantic;

public sealed class ErrorValue : IValue
{
    public static readonly ErrorValue Instance = new();
    
    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }

    public Range SourceRange { get; } = Range.All;
}