using System.Text;
using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree;

namespace unicfg.Base.Extensions;

public static class ElementExtensions
{
    public static string ToDisplayName(this INamedElement @this)
    {
        if (@this.Name.IsEmpty)
        {
            throw new InvalidOperationException();
        }

        var path = new Stack<ISymbol>();
        var parent = @this.Parent;
        var capacity = @this.Name.Length + 2;

        while (parent is { Name.IsEmpty: false })
        {
            path.Push(parent);
            capacity += parent.Name.Length + 1;
            parent = parent.Parent;
        }

        var builder = new StringBuilder(capacity);

        while (path.TryPop(out parent))
        {
            if (builder.Length > 0)
            {
                builder.Append('.');
            }

            builder.Append(parent.Name.ToString());
        }

        if (@this is AttributeElement)
        {
            builder.Append('[');
        }

        builder.Append(@this.Name.ToString());
        if (@this is AttributeElement)
        {
            builder.Append(']');
        }

        return builder.ToString();
    }

    public static SymbolRef GetSymbolRef(this ISymbol @this)
    {
        if (@this.Parent is null)
        {
            return SymbolRef.Null;
        }

        if (@this.Name.IsEmpty)
        {
            throw new InvalidOperationException();
        }

        var symbol = @this;
        var builder = ImmutableArray.CreateBuilder<StringRef>();

        do
        {
            builder.Insert(0, symbol.Name);
            symbol = symbol.Parent;
        } while (symbol is { Name.IsEmpty: false });

        return new SymbolRef(builder.ToImmutable());
    }
}
