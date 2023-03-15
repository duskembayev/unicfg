using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace unicfg.Cli;

internal sealed class CliLoggerFormatter : ConsoleFormatter
{
    private readonly InvocationContext _context;
    private ConsoleRenderer _renderer;
    private ScreenView _screenView;

    public CliLoggerFormatter(InvocationContext context) : base("CLI")
    {
        _context = context;
        _renderer = new ConsoleRenderer(_context.Console.GetTerminal(), OutputMode.NonAnsi, true);
        _renderer.Formatter.AddFormatter<LogLevel>(FormatLevel);

        _screenView = new ScreenView(_renderer, context.Console);
    }

    public override void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider scopeProvider,
        TextWriter textWriter)
    {
        var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception);
        _screenView.Child = new ContentView()
        FormattableString formattableString = $"{logEntry.LogLevel}{message}";
        _renderer.Append(formattableString);
    }

    private static TextSpan FormatLevel(LogLevel level)
    {
        var foreground = ForegroundColorSpan.Reset();
        var background = BackgroundColorSpan.Reset();

        switch (level)
        {
            case LogLevel.Trace:
                foreground = ForegroundColorSpan.DarkGray();
                break;
            case LogLevel.Debug:
                foreground = ForegroundColorSpan.LightGray();
                break;
            case LogLevel.Information:
                break;
            case LogLevel.Warning:
                foreground = ForegroundColorSpan.LightYellow();
                break;
            case LogLevel.Error:
                foreground = ForegroundColorSpan.Red();
                break;
            case LogLevel.Critical:
                foreground = ForegroundColorSpan.White();
                background = BackgroundColorSpan.Red();
                break;
            case LogLevel.None:
            default:
                throw new ArgumentOutOfRangeException(nameof(level), level, null);
        }

        return new ContainerSpan(foreground, background);
    }
}