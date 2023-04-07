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

    public void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }
}