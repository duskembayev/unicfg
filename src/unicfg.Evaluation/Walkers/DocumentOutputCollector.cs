using unicfg.Base.SyntaxTree.Walkers;

namespace unicfg.Evaluation.Walkers;

internal sealed class DocumentOutputCollector : AsyncWalker
{
    private readonly List<StringRef> _path;
    private readonly ImmutableHashSet<DocumentOutput>.Builder _result;

    public DocumentOutputCollector(CancellationToken cancellationToken)
        : base(cancellationToken)
    {
        _result = ImmutableHashSet.CreateBuilder<DocumentOutput>();
        _path = new List<StringRef>();
    }

    public ImmutableHashSet<DocumentOutput> GetResult()
    {
        return _result.ToImmutable();
    }

    public override async ValueTask Visit(ScopeSymbol scope)
    {
        if (!scope.IsRoot)
        {
            _path.Add(scope.Name);
        }

        await base.Visit(scope).ConfigureAwait(false);

        if (!scope.IsRoot)
        {
            _path.RemoveAt(_path.Count - 1);
        }
    }

    public override ValueTask Visit(PropertySymbol property)
    {
        return ValueTask.CompletedTask;
    }

    public override ValueTask Visit(AttributeElement attribute)
    {
        if (!attribute.Name.Equals(Attributes.Output))
        {
            return ValueTask.CompletedTask;
        }

        var scopeRef = _path.Count > 0
            ? new SymbolRef(_path.ToImmutableArray())
            : SymbolRef.Null;

        _result.Add(new DocumentOutput(scopeRef));
        return ValueTask.CompletedTask;
    }
}
