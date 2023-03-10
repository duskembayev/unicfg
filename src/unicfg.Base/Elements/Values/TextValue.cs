using unicfg.Base.Primitives;

namespace unicfg.Base.Elements.Values;

public sealed class TextValue : IValue
{
    public TextValue(Range sourceRange, StringRef text)
    {
        SourceRange = sourceRange;
        Text = text;
    }

    public Range SourceRange { get; }
    public StringRef Text { get; }

    public void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }
}