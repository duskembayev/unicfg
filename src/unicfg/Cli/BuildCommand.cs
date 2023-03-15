using System.CommandLine;

namespace unicfg.Cli;

internal class BuildCommand : Command
{
    public BuildCommand() : base("build", "unicfg Builder")
    {
        Add(CliSymbols.InputsArgument);
        Add(CliSymbols.OutputDirOption);
        Add(CliSymbols.PropertiesOption);
    }
}