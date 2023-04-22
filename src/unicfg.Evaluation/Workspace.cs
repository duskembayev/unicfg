using unicfg.Base.Analysis;
using unicfg.Base.Formatters;
using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;
using unicfg.Base.SyntaxTree;
using unicfg.Evaluation.Extensions;
using unicfg.Evaluation.Outputs;

namespace unicfg.Evaluation;

[ContainerEntry(ServiceLifetime.Scoped, typeof(IWorkspace))]
public sealed class Workspace : IWorkspace
{
    private readonly Dictionary<SymbolRef, StringRef> _defaults;
    private readonly Diagnostics _diagnostics;
    private readonly IDocumentResolver _documentResolver;
    private readonly List<Document> _entries;
    private readonly HashSet<IFormatter> _formatters;
    private readonly HashSet<DocumentOutput> _outputs;
    private readonly Dictionary<SymbolRef, StringRef> _overrides;
    private readonly Dictionary<DocumentKey, Document> _registry;

    public Workspace(IDocumentResolver documentResolver, Diagnostics diagnostics)
    {
        _documentResolver = documentResolver;
        _diagnostics = diagnostics;

        _overrides = new Dictionary<SymbolRef, StringRef>();
        _defaults = new Dictionary<SymbolRef, StringRef>();
        _registry = new Dictionary<DocumentKey, Document>();
        _entries = new List<Document>();
        _outputs = new HashSet<DocumentOutput>();
        _formatters = new HashSet<IFormatter>();
    }

    public ISet<DocumentOutput> Outputs => _outputs;
    public ISet<IFormatter> Formatters => _formatters;

    public async Task OpenFromAsync(string filePath, CancellationToken cancellationToken)
    {
        var document = await _documentResolver
                             .LoadFromFileAsync(filePath, DocumentFormat.Uni, cancellationToken)
                             .ConfigureAwait(false);

        await OpenAsync(document, cancellationToken).ConfigureAwait(false);
    }

    public async Task OpenAsync(Document document, CancellationToken cancellationToken)
    {
        if (document.Location is null)
        {
            throw new InvalidOperationException();
        }

        _outputs.UnionWith(await document.GetOutputsAsync(cancellationToken).ConfigureAwait(false));
        _registry.Add(DocumentKey.FromLocation(document.Location), document);
        _entries.Add(document);
    }

    public void DefaultPropertyValue(SymbolRef propertyPath, StringRef value)
    {
        _defaults[propertyPath] = value;
    }

    public void OverridePropertyValue(SymbolRef propertyPath, StringRef value)
    {
        _overrides[propertyPath] = value;
    }

    public async Task<ImmutableArray<EmitResult>> EmitAsync(CancellationToken cancellationToken)
    {
        if (_outputs.Count == 0)
        {
            _diagnostics.Report(DiagnosticDescriptor.NothingToEmit);
            return ImmutableArray<EmitResult>.Empty;
        }

        var evaluationContext = new EvaluationContext(
            _entries.ToImmutableArray(),
            _registry.ToImmutableDictionary(),
            _defaults.ToImmutableDictionary(),
            _overrides.ToImmutableDictionary(),
            _formatters.ToImmutableArray());

        var emitting =
            _outputs.Select(output => EmitDocumentAsync(output.ScopeRef, evaluationContext, cancellationToken));
        var results = await Task.WhenAll(emitting).ConfigureAwait(false);

        return results.ToImmutableArray();
    }

    private async Task<EmitResult> EmitDocumentAsync(
        SymbolRef scopeRef,
        EvaluationContext evaluationContext,
        CancellationToken cancellationToken)
    {
        var scope = await BuildScopeAsync(scopeRef, evaluationContext, cancellationToken).ConfigureAwait(false);
        var formatter = evaluationContext.Formatters.FirstOrDefault(f => f.Matches(scope.Attributes));

        if (formatter is null)
        {
            throw new NotImplementedException();
        }

        return await formatter
                     .FormatAsync(scopeRef, scope, cancellationToken)
                     .ConfigureAwait(false);
    }

    private static async Task<EmitScope> BuildScopeAsync(
        SymbolRef scopeRef,
        EvaluationContext evaluationContext,
        CancellationToken cancellationToken)
    {
        var valueEvaluator = new ValueEvaluator(evaluationContext.Entries, evaluationContext.Overrides);
        var outputBuilder = new OutputBuilder(valueEvaluator, evaluationContext.Defaults, cancellationToken);

        foreach (var entry in evaluationContext.Entries)
        {
            var symbol = entry.FindSymbol(scopeRef);

            if (symbol is not null)
            {
                await symbol.Accept(outputBuilder).ConfigureAwait(false);
            }
        }

        return outputBuilder.Scope;
    }
}
