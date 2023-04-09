using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Base.SyntaxTree;

public interface IElementWithValue : IElement
{
    IValue Value { get; }
}