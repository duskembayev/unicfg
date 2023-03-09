using System.Collections.Immutable;
using unicfg.Model.Elements;
using unicfg.Model.Elements.Values;

namespace unicfg.Evaluation;

internal sealed class PropertyRefCollector : AbstractElementVisitor
{
    private readonly ImmutableArray<RefValue>.Builder _result;

    public PropertyRefCollector()
    {
        _result = ImmutableArray.CreateBuilder<RefValue>();
    }

    public ImmutableArray<RefValue> GetResult() => _result.ToImmutable();

    public override void Visit(RefValue refValue)
    {
        _result.Add(refValue);
    }
}