using unicfg.Base.Extensions;
using unicfg.Base.Primitives;

namespace unicfg.Base.SyntaxTree;

public sealed class Document : IElement
{
    private ScopeSymbol? _rootGroup;

    internal Document(string baseDirectory, string? location)
    {
        BaseDirectory = baseDirectory;
        Location = location;
    }

    public string? Location { get; }
    public string BaseDirectory { get; }

    public ScopeSymbol RootScope
    {
        get => _rootGroup ?? throw new InvalidOperationException();
        internal set => _rootGroup = value;
    }

    public void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }

    public ISymbol? FindSymbol(SymbolRef symbolRef)
    {
        if (symbolRef.Path.Length == 0)
            return RootScope;

        ISymbol result = RootScope;
        var depth = 0;

        do
        {
            result = ((ScopeSymbol) result).GetChildSymbol(symbolRef.Path[depth]);
        } while (++depth < symbolRef.Path.Length && result is ScopeSymbol);

        return depth == symbolRef.Path.Length ? result : null;
    }
}