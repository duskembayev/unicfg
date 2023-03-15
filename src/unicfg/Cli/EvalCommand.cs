using System.CommandLine;

namespace unicfg.Cli;

internal class EvalCommand : Command
{
    public EvalCommand() : base("eval", "unicfg Evaluator")
    {
        Add(CliSymbols.InputsArgument);
        Add(CliSymbols.SymbolsOption);
        Add(CliSymbols.PropertiesOption);
        Add(CliSymbols.NonameOption);
    }
}