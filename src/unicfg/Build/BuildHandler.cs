using System.CommandLine.Parsing;
using unicfg.Base.Primitives;
using unicfg.Cli;
using unicfg.Evaluation;

namespace unicfg.Build;

internal class BuildHandler : CliCommandHandler
{
    private readonly IWorkspace _workspace;
    private readonly ILogger<BuildHandler> _logger;

    public BuildHandler(IWorkspace workspace, ILogger<BuildHandler> logger) : base(logger)
    {
        _workspace = workspace;
        _logger = logger;
    }

    protected override async Task<ExitCode> InvokeAsync(CommandResult commandResult, CancellationToken cancellationToken)
    {
        var inputs = commandResult.GetValueForArgument(CliSymbols.InputsArgument);
        var properties = commandResult.GetValueForOption(CliSymbols.PropertiesOption);
        var outputDir = commandResult.GetValueForOption(CliSymbols.OutputDirOption);

        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentNullException.ThrowIfNull(outputDir);

        foreach (var file in inputs)
            _workspace.OpenFrom(file.FullName);
        
        foreach (var (name, value) in properties)
            _workspace.OverrideProperty(SymbolRef.FromPath(name), value);

        return ExitCode.Success;
    }
}