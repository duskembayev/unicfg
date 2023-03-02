using System.Collections.Immutable;
using unicfg.Model;
using unicfg.Model.Semantic;

namespace unicfg.Evaluator;

internal sealed class PropertyRefCollector : Walker
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