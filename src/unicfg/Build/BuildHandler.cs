using System.CommandLine.Parsing;
using unicfg.Base.Primitives;
using unicfg.Cli;
using unicfg.Evaluation;
using unicfg.Extensions;

namespace unicfg.Build;

internal sealed class BuildHandler : CliCommandHandler
{
    private readonly ILogger<BuildHandler> _logger;
    private readonly IWorkspace _workspace;

    public BuildHandler(IWorkspace workspace, ILogger<BuildHandler> logger) : base(logger)
    {
        _workspace = workspace;
        _logger = logger;
    }

    protected override async Task<ExitCode> InvokeAsync(
        CommandResult commandResult,
        TextWriter stdoutWriter,
        CancellationToken cancellationToken)
    {
        var inputs = commandResult.GetValueForArgument(CliSymbols.InputsArgument);
        var properties = commandResult.GetValueForOption(CliSymbols.PropertiesOption);
        var outputDir = commandResult.GetValueForOption(CliSymbols.OutputDirOption);

        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentNullException.ThrowIfNull(outputDir);

        foreach (var file in inputs)
        {
            await _workspace.OpenFromAsync(file.FullName, cancellationToken);
        }

        foreach (var (path, value) in properties)
        {
            _workspace.OverridePropertyValue(SymbolRef.FromPath(path), value);
        }

        var results = await _workspace.EmitAsync(cancellationToken);
        return results.Output("Build", _logger);
    }
}
