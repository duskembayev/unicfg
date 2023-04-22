using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;
using unicfg.Cli;

namespace unicfg.Extensions;

internal static class EmitResultExtensions
{
    internal static ExitCode OutputResults(
        this IReadOnlyCollection<EmitResult> results,
        string operation,
        ILogger logger)
    {
        if (results.Count == 0)
        {
            return ExitCode.NoResult;
        }

        var errors = 0;

        foreach (var emitResult in results)
        {
            if (emitResult.HasErrors)
            {
                logger.OutputErrorResult(emitResult, operation);
                errors++;
                continue;
            }

            logger.OutputResult(emitResult, operation);
        }

        return errors switch
        {
            > 0 when errors < results.Count => ExitCode.PartialError,
            > 0 => ExitCode.Error,
            _ => ExitCode.Success
        };
    }

    private static void OutputErrorResult(this ILogger logger, EmitResult emitResult, string operation)
    {
        logger.LogError(
            "{OPERATION} failed: {SCOPE} -> {FILE} ({ERRORS} errors of {TOTAL} properties)",
            operation,
            emitResult.Scope == SymbolRef.Null ? "ROOT" : emitResult.Scope,
            emitResult.OutputPath,
            emitResult.ErrorPropertyCount,
            emitResult.TotalPropertyCount);
    }

    private static void OutputResult(this ILogger logger, EmitResult emitResult, string operation)
    {
        logger.LogInformation(
            "{OPERATION} succeeded: {SCOPE} -> {FILE} ({TOTAL} properties)",
            operation,
            emitResult.Scope == SymbolRef.Null ? "ROOT" : emitResult.Scope,
            emitResult.OutputPath,
            emitResult.TotalPropertyCount);
    }
}
