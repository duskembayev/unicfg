using System.Text;
using unicfg.Model.Semantic;
using Attribute = unicfg.Model.Semantic.Attribute;

namespace unicfg.Model.Extensions;

public static class SemanticModelExtensions
{
    public static string ToDisplayName(this SemanticNodeWithName @this)
    {
        var builder = @this is Attribute
            ? new StringBuilder('[' + @this.Name.ToString() + ']')
            : new StringBuilder(@this.Name.ToString());

        var parent = @this.Parent;

        while (parent is {Name.Length: > 0})
        {
            builder.Insert(0, '.');
            builder.Insert(0, parent.Name.Span);
            parent = parent.Parent;
        }

        return builder.ToString();
    }
}