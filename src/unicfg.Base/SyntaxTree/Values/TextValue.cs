using unicfg.Base.Primitives;

namespace unicfg.Base.SyntaxTree.Values;

public sealed class TextValue : IValue
{
    public TextValue(Range sourceRange, StringRef text)
    {
        SourceRange = sourceRange;
        Text = text;
    }

    public StringRef Text { get; }

    public Range SourceRange { get; }

    public T Accept<T>(IElementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
