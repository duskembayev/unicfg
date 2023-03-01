using System.Text;
using ns2x.Model.Semantic;
using Attribute = ns2x.Model.Semantic.Attribute;

namespace ns2x.Model.Extensions;

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