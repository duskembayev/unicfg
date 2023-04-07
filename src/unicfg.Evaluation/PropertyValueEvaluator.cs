using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree.Values;
using unicfg.Evaluation.Walkers;

namespace unicfg.Evaluation;

internal sealed class PropertyValueEvaluator : AbstractWalker
{
    private readonly IReadOnlyDictionary<SymbolRef, StringRef> _dependencyValues;

    public PropertyValueEvaluator(IReadOnlyDictionary<SymbolRef, StringRef> dependencyValues)
    {
        _dependencyValues = dependencyValues;
        Result = StringRef.Empty;
    }

    public bool HasErrors { get; private set; }

    public StringRef Result { get; private set; }

    public override void Visit(TextValue textValue)
    {
        Result += textValue.Text;
    }

    public override void Visit(RefValue refValue)
    {
        Result += _dependencyValues[refValue.Property];
    }

    public override void Visit(ErrorValue errorValue)
    {
        HasErrors = true;
    }
}