using System.Diagnostics.CodeAnalysis;
using unicfg.Base.Primitives;

namespace unicfg.Base.SyntaxTree;

public sealed class ScopeSymbol : ISymbol
{
    internal ScopeSymbol(StringRef name, ISymbol? parent, Document document)
    {
        Name = name;
        Parent = parent;
        Document = document;

        Children = ImmutableDictionary<StringRef, ISymbol>.Empty;
        Attributes = ImmutableDictionary<StringRef, AttributeElement>.Empty;
    }

    [MemberNotNullWhen(false, nameof(Parent))]
    public bool IsRoot => Parent is null;

    public ImmutableDictionary<StringRef, ISymbol> Children { get; internal set; }
    public ImmutableDictionary<StringRef, AttributeElement> Attributes { get; internal set; }

    public StringRef Name { get; }
    public ISymbol? Parent { get; }
    public Document Document { get; }

    public T Accept<T>(IElementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
