using System.CommandLine;

namespace unicfg.Cli;

public static class Commands
{
    public static RootCommand RootCommand()
    {
        var rootCommand = new RootCommand("unicfg CLI")
        {
            BuildCommand(),
        };

        rootCommand.AddGlobalOption(VerbosityOption());
        return rootCommand;
    }

    private static Option VerbosityOption()
    {
        var option = new Option<string>(new[] { "-v", "--verbosity" })
        {
            Description = "Set the verbosity level.",
            ArgumentHelpName = "q|m|n|d|diag"
        };

        return option;
    }

    private static Command BuildCommand()
    {
        var files = new Argument<FileInfo>("file")
        {
            Arity = ArgumentArity.OneOrMore,
            Description = "One or more files in uni-format to operate on.",
            HelpName = "file"
        };

        var output = new Option<DirectoryInfo>(new[] { "-o", "--output" })
        {
            Arity = ArgumentArity.ZeroOrOne,
            Description = "The output directory to place built artifacts in.",
            ArgumentHelpName = "outputDir"
        };

        var properties = new Option<string>(new[] { "-p", "--property" })
        {
            Arity = ArgumentArity.ZeroOrMore,
            Description = "Set or override the specified properties. Specify each property separately, or use a semicolon or comma to separate multiple properties",
            ArgumentHelpName = "propertyName=propertyValue"
        };

        return new Command("build", "unicfg Builder")
        {
            files,
            output,
            properties
        };
    }
}