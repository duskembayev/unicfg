using unicfg.Base.Extensions;
using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;

namespace unicfg.Evaluation.Formatter;

public sealed class EvaluationAsyncVisitor : IEmitAsyncVisitor
{
    private const char Delimiter = '.';
    private const char Equality = '=';
    private readonly List<StringRef> _path;
    private readonly TextWriter _writer;

    public EvaluationAsyncVisitor(TextWriter writer)
    {
        _writer = writer;
        _path = new List<StringRef>();
        
        TotalPropertyCount = 0;
        SuccessPropertyCount = 0;
    }

    public int TotalPropertyCount { get; private set; }
    public int SuccessPropertyCount { get; private set; }

    async ValueTask IEmitAsyncVisitor.VisitScopeAsync(EmitScope scope, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _path.Add(scope.Name);
        await AcceptChildrenAsync(scope, cancellationToken).ConfigureAwait(false);
        _path.RemoveAt(_path.Count - 1);
    }

    async ValueTask IEmitAsyncVisitor.VisitPropertyAsync(EmitProperty property, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        for (var i = 0; i < _path.Count; i++)
        {
            await _writer.WriteAsync(_path[i], cancellationToken).ConfigureAwait(false);
            await _writer.WriteAsync(Delimiter).ConfigureAwait(false);
        }

        await _writer.WriteAsync(property.Name, cancellationToken).ConfigureAwait(false);
        await _writer.WriteAsync(Equality).ConfigureAwait(false);

        if (property.Value is {State: EvaluationState.Evaluated})
        {
            await _writer.WriteAsync(property.Value.Value, cancellationToken).ConfigureAwait(false);
            SuccessPropertyCount++;
        }

        await _writer.WriteLineAsync().ConfigureAwait(false);
        TotalPropertyCount++;
    }

    private async ValueTask AcceptChildrenAsync(EmitScope scope, CancellationToken cancellationToken)
    {
        foreach (var (_, childScope) in scope.Scopes)
            await childScope.AcceptAsync(this, cancellationToken).ConfigureAwait(false);

        foreach (var (_, childProperty) in scope.Properties)
            await childProperty.AcceptAsync(this, cancellationToken).ConfigureAwait(false);
    }
}