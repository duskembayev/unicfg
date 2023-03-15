using System.CommandLine;
using System.CommandLine.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace unicfg.Cli;

public static class CliExtensions
{
    internal static IHostBuilder ConfigureLoggingByVerbosity(this IHostBuilder builder)
    {
        var context = builder.GetInvocationContext();
        var verbosityLevel = context.ParseResult.GetValueForOption(Verbosity.Option);
        var logLevel = verbosityLevel switch
        {
            Verbosity.Level.Quiet => LogLevel.Error,
            Verbosity.Level.Minimal => LogLevel.Warning,
            Verbosity.Level.Normal => LogLevel.Information,
            Verbosity.Level.Detailed => LogLevel.Debug,
            Verbosity.Level.Diagnostic => LogLevel.Trace,
            _ => throw new ArgumentOutOfRangeException()
        };

        builder.ConfigureLogging(loggingBuilder => loggingBuilder
            .SetMinimumLevel(logLevel)
            .AddConsoleFormatter<CliLoggerFormatter, CliLoggerFormatterOptions>(options => options.IncludeScopes = true)
            .AddConsole(options => options.FormatterName = "CLI"));

        return builder;
    }
}