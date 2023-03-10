using unicfg.Base.Primitives;

namespace unicfg.Base.Elements;

public abstract class UniSymbol : ElementWithName
{
    protected UniSymbol(StringRef name, Document document, ElementWithName? parent)
        : base(name, document, parent)
    {
    }

    public ImmutableArray<UniAttribute> Attributes { get; internal set; }
}