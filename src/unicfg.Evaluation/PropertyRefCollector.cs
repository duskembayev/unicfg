using unicfg.Base.SyntaxTree.Values;
using unicfg.Evaluation.Walkers;

namespace unicfg.Evaluation;

internal sealed class PropertyRefCollector : AbstractWalker
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