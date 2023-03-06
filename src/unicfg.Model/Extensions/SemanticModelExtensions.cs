using System.Text;
using unicfg.Model.Semantic;
using Attribute = unicfg.Model.Semantic.Attribute;

namespace unicfg.Model.Extensions;

public static class SemanticModelExtensions
{
    public static string ToDisplayName(this SemanticNodeWithName @this)
    {
        if (@this.Name.IsEmpty)
            throw new InvalidOperationException();

        var path = new Stack<SemanticNodeWithName>();
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

        if (@this is Attribute) builder.Append('[');
        builder.Append(@this.Name.ToString());
        if (@this is Attribute) builder.Append(']');

        return builder.ToString();
    }
}