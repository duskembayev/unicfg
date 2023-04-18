using unicfg.Base.Primitives;

namespace unicfg.Base.SyntaxTree.Values;

public sealed class RefValue : IValue
{
    public RefValue(Range sourceRange, SymbolRef property)
    {
        SourceRange = sourceRange;
        Property = property;
    }

    public Range SourceRange { get; }
    public SymbolRef Property { get; }

    public T Accept<T>(IElementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}