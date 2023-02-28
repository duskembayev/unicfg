using System.Text;
using ns2x.Model;
using ns2x.Model.Primitives;
using ns2x.Model.Semantic;

namespace ns2x.Evaluator;

internal sealed class PropertyValueEvaluator : Walker
{
    private readonly IReadOnlyDictionary<PropertyRef, string> _dependencyValues;
    private readonly StringBuilder _valueBuilder;

    public PropertyValueEvaluator(IReadOnlyDictionary<PropertyRef, string> dependencyValues)
    {
        _dependencyValues = dependencyValues;
        _valueBuilder = new StringBuilder();
    }

    public string GetResult() => _valueBuilder.ToString();

    public override void Visit(TextValue textValue)
    {
        _valueBuilder.Append(textValue.Text);
    }

    public override void Visit(RefValue refValue)
    {
        _valueBuilder.Append(_dependencyValues[refValue.Property]);
    }
}