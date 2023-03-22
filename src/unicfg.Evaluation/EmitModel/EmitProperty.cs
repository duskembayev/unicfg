using unicfg.Base.Primitives;

namespace unicfg.Evaluation.EmitModel;

public sealed record EmitProperty
{
    public IReadOnlyDictionary<StringRef, StringRef> Attributes { get; set; }
    public StringRef Value { get; set; }
}