using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;

namespace unicfg.Cli;

internal abstract class CliCommandHandler : ICommandHandler
{
    private readonly ILogger _logger;

    protected CliCommandHandler(ILogger logger)
    {
        _logger = logger;
    }

    public int Invoke(InvocationContext context)
    {
        var invokeTask = InvokeAsync(context);

        invokeTask.Wait();

        return context.ExitCode;
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var cancellationToken = context.GetCancellationToken();
        var commandResult = context.BindingContext.ParseResult.RootCommandResult;
        await using var stdOutWriter = context.Console.Out.CreateTextWriter();

        ExitCode exitCode;

        try
        {
            exitCode = await InvokeAsync(commandResult, stdOutWriter, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Unhandled exception");
            exitCode = ExitCode.UnhandledException;
        }

        context.ExitCode = (int) exitCode;
        return context.ExitCode;
    }

    protected abstract Task<ExitCode> InvokeAsync(
        CommandResult commandResult,
        TextWriter stdOutWriter,
        CancellationToken cancellationToken);
}