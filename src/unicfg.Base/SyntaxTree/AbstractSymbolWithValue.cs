using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Base.SyntaxTree;

public abstract class AbstractSymbolWithValue : AbstractSymbol
{
    protected AbstractSymbolWithValue(StringRef name, Document document, AbstractSymbol parent)
        : base(name, document, parent)
    {
    }

    public IValue Value => Values[^1];
    public ImmutableArray<IValue> Values { get; internal set; }
}