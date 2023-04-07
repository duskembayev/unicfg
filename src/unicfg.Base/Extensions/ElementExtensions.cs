using System.Text;
using unicfg.Base.SyntaxTree;

namespace unicfg.Base.Extensions;

public static class ElementExtensions
{
    public static string ToDisplayName(this AbstractSymbol @this)
    {
        if (@this.Name.IsEmpty)
            throw new InvalidOperationException();

        var path = new Stack<AbstractSymbol>();
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
                builder.Append('.');

            builder.Append(parent.Name.ToString());
        }

        if (@this is AttributeSymbol) builder.Append('[');
        builder.Append(@this.Name.ToString());
        if (@this is AttributeSymbol) builder.Append(']');

        return builder.ToString();
    }
}