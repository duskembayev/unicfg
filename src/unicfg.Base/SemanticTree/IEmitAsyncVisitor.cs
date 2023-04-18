namespace unicfg.Base.SemanticTree;

public interface IEmitAsyncVisitor
{
    ValueTask VisitScopeAsync(EmitScope scope, CancellationToken cancellationToken);
    ValueTask VisitPropertyAsync(EmitProperty property, CancellationToken cancellationToken);
}