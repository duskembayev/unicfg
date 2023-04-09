using unicfg.Base.Primitives;

namespace unicfg.Base.SemanticTree;

public sealed class EmitProperty : EmitSymbol
{
    public EmitProperty(StringRef name) : base(name)
    {
    }

    public EmitValue? Value { get; set; }

    public override ValueTask AcceptAsync(IEmitAsyncVisitor visitor, CancellationToken cancellationToken)
    {
        return visitor.VisitPropertyAsync(this, cancellationToken);
    }
}