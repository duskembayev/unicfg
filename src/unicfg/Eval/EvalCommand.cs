using System.CommandLine;
using unicfg.Cli;

namespace unicfg.Eval;

internal class EvalCommand : Command
{
    public EvalCommand() : base("eval", "unicfg Evaluator")
    {
        Add(CliSymbols.InputsArgument);
        Add(CliSymbols.SymbolsOption);
        Add(CliSymbols.PropertiesOption);
    }
}