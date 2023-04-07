using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree;

namespace unicfg.Evaluation;

public sealed class PropertyResolver : IPropertyResolver
{
    private readonly Document _document;

    public PropertyResolver(Document document)
    {
        _document = document;
    }

    public PropertySymbol? ResolveProperty(SymbolRef propertyRef)
    {
        return _document.FindProperty(propertyRef);
    }
}