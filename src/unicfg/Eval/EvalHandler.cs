using System.Collections.Immutable;
using System.CommandLine.Parsing;
using unicfg.Base.Primitives;
using unicfg.Cli;
using unicfg.Evaluation;

namespace unicfg.Eval;

internal class EvalHandler : CliCommandHandler
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
        TextWriter textWriter,
        CancellationToken cancellationToken)
    {
        var inputs = commandResult.GetValueForArgument(CliSymbols.InputsArgument);
        var symbols = commandResult.GetValueForOption(CliSymbols.SymbolsOption);
        var properties = commandResult.GetValueForOption(CliSymbols.PropertiesOption);
        var noname = commandResult.GetValueForOption(CliSymbols.NonameOption);

        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(symbols);
        ArgumentNullException.ThrowIfNull(properties);

        foreach (var file in inputs)
            _workspace.OpenFrom(file.FullName);

        foreach (var (path, value) in properties)
            _workspace.OverrideProperty(SymbolRef.FromPath(path), value);

        var outputs = symbols
            .Select(info => new DocumentOutput(SymbolRef.FromPath(info.Path)))
            .ToImmutableArray();

        _workspace.Outputs.Clear();
        _workspace.Outputs.UnionWith(outputs);

        _workspace.Formatters.Clear();
        _workspace.Formatters.Add(new EvaluationFormatter(textWriter, ));

        var results = await _workspace.EmitAsync(cancellationToken);

        if (symbols.Count == 0)
        {
            _
        }

        return ExitCode.Success;
    }
}