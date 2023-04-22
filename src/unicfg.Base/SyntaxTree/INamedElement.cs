using unicfg.Base.Primitives;

namespace unicfg.Base.SyntaxTree;

public interface INamedElement : IElement
{
    ISymbol? Parent { get; }
    StringRef Name { get; }
}
