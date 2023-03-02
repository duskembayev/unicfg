using System.Text;
using unicfg.Model;
using unicfg.Model.Primitives;
using unicfg.Model.Semantic;

namespace unicfg.Evaluator;

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