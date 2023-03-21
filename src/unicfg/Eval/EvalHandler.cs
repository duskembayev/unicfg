using System.CommandLine.Parsing;
using unicfg.Cli;

namespace unicfg.Eval;

internal class EvalHandler : CliCommandHandler
{
    private readonly ILogger<EvalHandler> _logger;

    public EvalHandler(ILogger<EvalHandler> logger) : base(logger)
    {
        _logger = logger;
    }

    protected override async Task<ExitCode> InvokeAsync(CommandResult commandResult, CancellationToken cancellationToken)
    {
        var inputs = commandResult.GetValueForArgument(CliSymbols.InputsArgument);
        var symbols = commandResult.GetValueForOption(CliSymbols.SymbolsOption);
        var properties = commandResult.GetValueForOption(CliSymbols.PropertiesOption);
        var noname = commandResult.GetValueForOption(CliSymbols.NonameOption);

        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(symbols);
        ArgumentNullException.ThrowIfNull(properties);

        
        
        return ExitCode.Success;
    }
}