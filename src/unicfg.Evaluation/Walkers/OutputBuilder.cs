using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;
using unicfg.Base.SyntaxTree;

namespace unicfg.Evaluation.Walkers;

internal class OutputBuilder : AbstractWalker
{
    private readonly SymbolRef _targetScope;
    private readonly EmitScope _outputScope;

    public OutputBuilder(SymbolRef targetScope, CancellationToken cancellationToken)
    {
        _targetScope = targetScope;
        _outputScope = new EmitScope();
    }

    public EmitScope Scope { get; }

    public override void Visit(Document document)
    {
        document.FindProperty()
    }

    public override void Visit(ScopeSymbol scope)
    {
        
        
        base.Visit(scope);
    }
}