using System.CommandLine.Parsing;
using unicfg.Base.Primitives;
using unicfg.Cli;
using unicfg.Evaluation;
using unicfg.Evaluation.EmitModel;

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

    protected override async Task<ExitCode> InvokeAsync(
        CommandResult commandResult,
        CancellationToken cancellationToken)
    {
        var inputs = commandResult.GetValueForArgument(CliSymbols.InputsArgument);
        var properties = commandResult.GetValueForOption(CliSymbols.PropertiesOption);
        var outputDir = commandResult.GetValueForOption(CliSymbols.OutputDirOption);

        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentNullException.ThrowIfNull(outputDir);

        foreach (var file in inputs)
            _workspace.OpenFrom(file.FullName);

        foreach (var (path, value) in properties)
            _workspace.OverrideProperty(SymbolRef.FromPath(path), value);

        var results = await _workspace.EmitAllAsync(outputDir.FullName, cancellationToken);

        if (results.Length == 0)
            return ExitCode.NoResult;

        var errors = 0;

        foreach (var emitResult in results)
        {
            if (emitResult.HasErrors)
            {
                OutputErrorResult(emitResult);
                errors++;
                continue;
            }

            OutputResult(emitResult);
        }

        if (errors > 0 && errors < results.Length)
            return ExitCode.PartialError;

        return errors == 0 ? ExitCode.Success : ExitCode.Error;
    }

    private void OutputErrorResult(EmitResult emitResult)
    {
        _logger.LogError("Build failed: {SCOPE} -> {FILE} ({ERRORS}/{TOTAL})",
            emitResult.Scope,
            emitResult.OutputPath,
            emitResult.ErrorPropertyCount,
            emitResult.TotalPropertyCount);
    }

    private void OutputResult(EmitResult emitResult)
    {
        _logger.LogInformation("Build succeeded: {SCOPE} -> {FILE} ({TOTAL})",
            emitResult.Scope,
            emitResult.OutputPath,
            emitResult.TotalPropertyCount);
    }
}