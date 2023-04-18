using System.CommandLine;
using unicfg.Cli;

namespace unicfg.Build;

internal class BuildCommand : Command
{
    public BuildCommand() : base("build", "unicfg Builder")
    {
        Add(CliSymbols.InputsArgument);
        Add(CliSymbols.OutputDirOption);
        Add(CliSymbols.PropertiesOption);
    }
}