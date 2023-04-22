using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;
using unicfg.Cli;

namespace unicfg.Extensions;

internal static class LoggerExtensions
{
    internal static ExitCode OutputResults(this ILogger logger, IReadOnlyCollection<EmitResult> results,
        string operation)
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

        if (errors > 0 && errors < results.Count)
        {
            return ExitCode.PartialError;
        }

        if (errors == results.Count)
        {
            return ExitCode.Error;
        }

        return ExitCode.Success;
    }

    private static void OutputErrorResult(this ILogger logger, EmitResult emitResult, string operation)
    {
        logger.LogError("{OPERATION} failed: {SCOPE} -> {FILE} ({ERRORS} errors of {TOTAL} properties)",
            operation,
            emitResult.Scope == SymbolRef.Null ? "ROOT" : emitResult.Scope,
            emitResult.OutputPath,
            emitResult.ErrorPropertyCount,
            emitResult.TotalPropertyCount);
    }

    private static void OutputResult(this ILogger logger, EmitResult emitResult, string operation)
    {
        logger.LogInformation("{OPERATION} succeeded: {SCOPE} -> {FILE} ({TOTAL} properties)",
            operation,
            emitResult.Scope == SymbolRef.Null ? "ROOT" : emitResult.Scope,
            emitResult.OutputPath,
            emitResult.TotalPropertyCount);
    }
}
