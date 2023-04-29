using System.Collections.Immutable;
using System.CommandLine.Parsing;
using Enhanced.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using unicfg.Base.Primitives;
using unicfg.Cli;
using unicfg.Evaluation;
using unicfg.Extensions;

namespace unicfg.Eval;

internal sealed class EvalHandler : CliCommandHandler
{
    private readonly ILogger<EvalHandler> _logger;
    private readonly IWorkspace _workspace;

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
        {
            await _workspace.OpenFromAsync(file.FullName, cancellationToken);
        }

        foreach (var (path, value) in properties)
        {
            _workspace.OverridePropertyValue(SymbolRef.FromPath(path), value);
        }

        var outputs = symbols
            .Select(info => new DocumentOutput(ParseSymbolRef(info)))
            .ToImmutableArray();

        // Remove all detected outputs and add the ones specified by the user.
        _workspace.Outputs.Clear();
        _workspace.Outputs.UnionWith(outputs);

        // Remove all embedded formatters and add the one that writes to STDOUT.
        _workspace.Formatters.Clear();
        _workspace.Formatters.Add(new EvalFormatter("STDOUT", stdOutWriter));

        var results = await _workspace.EmitAsync(cancellationToken);
        return results.Output("Evaluation", _logger);
    }

    private static SymbolRef ParseSymbolRef(SymbolInfo info)
    {
        return info == SymbolInfo.Root ? SymbolRef.Null : SymbolRef.FromPath(info.Path);
    }
}
