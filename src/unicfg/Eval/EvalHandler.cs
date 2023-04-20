using System.Collections.Immutable;
using System.CommandLine.Parsing;
using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;
using unicfg.Cli;
using unicfg.Evaluation;
using unicfg.Evaluation.Formatter;
using unicfg.Extensions;

namespace unicfg.Eval;

internal sealed class EvalHandler : CliCommandHandler
{
    private readonly IWorkspace _workspace;
    private readonly ILogger<EvalHandler> _logger;

    public EvalHandler(IWorkspace workspace, ILogger<EvalHandler> logger) : base(logger)
    {
        _workspace = workspace;
        _logger = logger;
    }

    protected override async Task<ExitCode> InvokeAsync(
        CommandResult commandResult,
        TextWriter stdOutWriter,
        CancellationToken cancellationToken)
    {
        var inputs = commandResult.GetValueForArgument(CliSymbols.InputsArgument);
        var symbols = commandResult.GetValueForOption(CliSymbols.SymbolsOption);
        var properties = commandResult.GetValueForOption(CliSymbols.PropertiesOption);

        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(symbols);
        ArgumentNullException.ThrowIfNull(properties);

        foreach (var file in inputs)
            await _workspace.OpenFromAsync(file.FullName, cancellationToken);

        foreach (var (path, value) in properties)
            _workspace.OverrideProperty(SymbolRef.FromPath(path), value);

        var outputs = symbols
            .Select(info => new DocumentOutput(ParseSymbolRef(info)))
            .ToImmutableArray();

        _workspace.Outputs.Clear();
        _workspace.Outputs.UnionWith(outputs);

        _workspace.Formatters.Clear();
        _workspace.Formatters.Add(new EvaluationFormatter("STDOUT", stdOutWriter));

        var results = await _workspace.EmitAsync(cancellationToken);

        return _logger.OutputResults(results, "Evaluation");
    }

    private static SymbolRef ParseSymbolRef(SymbolInfo info)
    {
        return info == SymbolInfo.Root ? SymbolRef.Null : SymbolRef.FromPath(info.Path);
    }
}