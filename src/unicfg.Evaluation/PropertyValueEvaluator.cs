using System.Text;
using unicfg.Model.Elements;
using unicfg.Model.Elements.Values;
using unicfg.Model.Primitives;

namespace unicfg.Evaluation;

internal sealed class PropertyValueEvaluator : AbstractElementVisitor
{
    private readonly IReadOnlyDictionary<PropertyRef, string> _dependencyValues;
    private readonly StringBuilder _valueBuilder;

    public PropertyValueEvaluator(IReadOnlyDictionary<PropertyRef, string> dependencyValues)
    {
        _dependencyValues = dependencyValues;
        _valueBuilder = new StringBuilder();
    }

    public bool HasErrors { get; private set; }

    public string GetResult() => _valueBuilder.ToString();

    public override void Visit(TextValue textValue)
    {
        _valueBuilder.Append(textValue.Text);
    }

    public override void Visit(RefValue refValue)
    {
        _valueBuilder.Append(_dependencyValues[refValue.Property]);
    }

    public override void Visit(ErrorValue errorValue)
    {
        HasErrors = true;
    }
}