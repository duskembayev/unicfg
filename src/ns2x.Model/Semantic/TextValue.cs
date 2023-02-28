namespace ns2x.Model.Semantic;

public sealed class TextValue : IValue
{
    public TextValue(Range sourceRange, StringRef text)
    {
        SourceRange = sourceRange;
        Text = text;
    }

    public Range SourceRange { get; }
    public StringRef Text { get; }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}