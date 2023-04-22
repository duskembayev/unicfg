using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Base.SyntaxTree;

public interface IElementWithValue : INamedElement
{
    IValue Value { get; }
}
