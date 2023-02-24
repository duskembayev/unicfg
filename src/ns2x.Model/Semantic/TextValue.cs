namespace ns2x.Model.Semantic;

public sealed class TextValue : IValue
{
    public TextValue(StringRef text)
    {
        Text = text;
    }

    public StringRef Text { get; }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}