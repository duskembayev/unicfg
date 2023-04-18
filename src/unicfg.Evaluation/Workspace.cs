using unicfg.Base.Analysis;
using unicfg.Base.Extensions;
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
    private readonly Dictionary<DocumentKey, Document> _registry;
    private readonly IDocumentResolver _documentResolver;
    private readonly Diagnostics _diagnostics;
    private readonly SortedList<int, Document> _entries;
    private readonly HashSet<DocumentOutput> _outputs;
    private readonly HashSet<IFormatter> _formatters;

    private int _priorityIndex;

    public Workspace(IDocumentResolver documentResolver, Diagnostics diagnostics)
    {
        _documentResolver = documentResolver;
        _diagnostics = diagnostics;
        _entries = new SortedList<int, Document>();
        _registry = new Dictionary<DocumentKey, Document>();
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
            throw new InvalidOperationException();

        _outputs.UnionWith(await document.GetOutputsAsync(cancellationToken).ConfigureAwait(false));
        _registry.Add(DocumentKey.FromLocation(document.Location), document);
        _entries.Add(_priorityIndex++, document);
    }

    public void OverrideProperty(SymbolRef property, StringRef value)
    {
        throw new NotImplementedException();
    }

    public async Task<ImmutableArray<EmitResult>> EmitAsync(CancellationToken cancellationToken)
    {
        if (_outputs.Count == 0)
        {
            _diagnostics.Report(DiagnosticDescriptor.NothingToEmit);
            return ImmutableArray<EmitResult>.Empty;
        }

        var evaluationContext = new EvaluationContext(
            _entries.ToImmutableArrayOfValues(),
            _registry.ToImmutableDictionary(),
            _formatters.ToImmutableArray());

        var emitting = _outputs.Select(output => EmitDocumentAsync(output, evaluationContext, cancellationToken));
        var results = await Task.WhenAll(emitting).ConfigureAwait(false);

        return results.ToImmutableArray();
    }

    private async Task<EmitResult> EmitDocumentAsync(
        DocumentOutput documentOutput,
        EvaluationContext evaluationContext,
        CancellationToken cancellationToken)
    {
        var valueEvaluator = new ValueEvaluator(evaluationContext);
        var outputBuilder = new OutputBuilder(valueEvaluator, cancellationToken);

        foreach (var entry in evaluationContext.Entries)
        {
            var symbol = entry.FindSymbol(documentOutput.ScopeRef);

            if (symbol is not null)
                await symbol.Accept(outputBuilder).ConfigureAwait(false);
        }

        var formatter = evaluationContext.Formatters
            .FirstOrDefault(f => f.Matches(outputBuilder.Scope.Attributes));

        if (formatter is null)
            throw new NotImplementedException();

        return await formatter
            .FormatAsync(documentOutput.ScopeRef, outputBuilder.Scope, cancellationToken)
            .ConfigureAwait(false);
    }
}