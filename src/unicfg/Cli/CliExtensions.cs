using System.CommandLine.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace unicfg.Cli;

internal static class CliExtensions
{
    internal static IHostBuilder ConfigureLoggingByVerbosity(this IHostBuilder builder)
    {
        var context = builder.GetInvocationContext();
        var verbosityLevel = context.ParseResult.GetValueForOption(Verbosity.Option);
        var logLevel = verbosityLevel switch
        {
            Verbosity.Level.Quiet => LogEventLevel.Error,
            Verbosity.Level.Minimal => LogEventLevel.Warning,
            Verbosity.Level.Normal => LogEventLevel.Information,
            Verbosity.Level.Detailed => LogEventLevel.Debug,
            Verbosity.Level.Diagnostic => LogEventLevel.Verbose,
            _ => throw new ArgumentOutOfRangeException()
        };

        builder.UseSerilog(
            (_, configuration) => configuration
                                  .MinimumLevel.Is(logLevel)
                                  .Enrich.FromLogContext()
                                  .WriteTo.Console(
                                      theme: AnsiConsoleTheme.Code,
                                      standardErrorFromLevel: LogEventLevel.Verbose));

        return builder;
    }
}
