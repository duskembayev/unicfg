using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;
using unicfg.Base.SyntaxTree;

namespace unicfg.Evaluation.Walkers;

internal class OutputBuilder : AbstractWalker
{
    private readonly ImmutableArray<StringRef> _targetPath;
    private readonly EmitScope _outputScope;
    private int _depth;

    public OutputBuilder(SymbolRef targetScope, CancellationToken cancellationToken)
    {
        _outputScope = new EmitScope();
        _targetPath = targetScope.Path;
        _depth = 0;
    }

    public EmitScope Scope { get; }

    public override void Visit(ScopeSymbol scope)
    {
        
        
        base.Visit(scope);
    }
}