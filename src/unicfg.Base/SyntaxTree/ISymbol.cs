using unicfg.Base.Primitives;

namespace unicfg.Base.SyntaxTree;

public interface ISymbol : INamedElement
{
    Document Document { get; }
    ImmutableDictionary<StringRef, AttributeElement> Attributes { get; }
}