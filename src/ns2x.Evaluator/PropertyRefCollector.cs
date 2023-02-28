using System.Collections.Immutable;
using ns2x.Model;
using ns2x.Model.Semantic;

namespace ns2x.Evaluator;

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