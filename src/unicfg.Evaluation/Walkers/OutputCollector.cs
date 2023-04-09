using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree;

namespace unicfg.Evaluation.Walkers;

internal sealed class OutputCollector : AbstractWalker
{
    private readonly Stack<StringRef> _path;
    private readonly ImmutableHashSet<DocumentOutput>.Builder _result;

    public OutputCollector()
    {
        _result = ImmutableHashSet.CreateBuilder<DocumentOutput>();
        _path = new Stack<StringRef>();
    }

    public ImmutableHashSet<DocumentOutput> GetResult()
    {
        return _result.ToImmutable();
    }

    public override void Visit(ScopeSymbol scope)
    {
        if (!scope.Name.IsEmpty)
            _path.Push(scope.Name);

        foreach (var attribute in scope.Attributes) attribute.Accept(this);
        foreach (var propertyGroup in scope.Scopes) propertyGroup.Accept(this);

        _path.TryPop(out _);
    }

    public override void Visit(AttributeElement attribute)
    {
        if (!attribute.Name.Equals(Attributes.Output))
            return;

        var groupRef = _path.Count > 0
            ? new SymbolRef(_path.Reverse().ToImmutableArray())
            : SymbolRef.Null;

        _result.Add(new DocumentOutput(groupRef));
    }
}