using System.CommandLine;
using System.CommandLine.Parsing;

namespace unicfg.Cli;

internal static class CliSymbols
{
    internal static readonly Argument<FileInfo> InputsArgument = CreateInputsArgument();
    internal static readonly Option<bool> NonameOption = CreateNonameOption();
    internal static readonly Option<DirectoryInfo> OutputDirOption = CreateOutputDirOption();
    internal static readonly Option<IReadOnlyList<PropertyInfo>> PropertiesOption = CreatePropertiesOption();
    internal static readonly Option<IReadOnlyList<SymbolInfo>> SymbolsOption = CreateSymbolsOption();

    private static Option<bool> CreateNonameOption()
    {
        return new Option<bool>("--noname")
        {
            Arity = ArgumentArity.Zero,
            Description = "Do not display the evaluated property names."
        };
    }

    private static Option<DirectoryInfo> CreateOutputDirOption()
    {
        return new Option<DirectoryInfo>(new[] { "-o", "--output" })
        {
            Arity = ArgumentArity.ExactlyOne,
            Description = "The output directory to place built artifacts in.",
            ArgumentHelpName = "OUTPUT_DIR"
        }.LegalFilePathsOnly();
    }

    private static Argument<FileInfo> CreateInputsArgument()
    {
        return new Argument<FileInfo>("FILE")
        {
            Arity = ArgumentArity.OneOrMore,
            Description = "One or more files in uni-format to operate on.",
            HelpName = "FILE"
        }.ExistingOnly();
    }

    private static Option<IReadOnlyList<SymbolInfo>> CreateSymbolsOption()
    {
        return new Option<IReadOnlyList<SymbolInfo>>(new[] { "-s", "--symbol" }, ParseSymbolsInfo)
        {
            Arity = ArgumentArity.OneOrMore,
            Description =
                "One or more symbols to evaluate. Specify each symbol separately, or use a semicolon or comma to separate multiple symbols.",
            ArgumentHelpName = "SYMBOL_PATH",
        };
    }

    private static Option<IReadOnlyList<PropertyInfo>> CreatePropertiesOption()
    {
        return new Option<IReadOnlyList<PropertyInfo>>(new[] { "-p", "--property" }, ParsePropertyInfo)
        {
            Arity = ArgumentArity.OneOrMore,
            Description =
                "Set or override the specified properties. Specify each property separately, or use a semicolon or comma to separate multiple properties.",
            ArgumentHelpName = "NAME=VALUE",
        };
    }

    private static IReadOnlyList<SymbolInfo> ParseSymbolsInfo(ArgumentResult result)
    {
        var symbols = new List<SymbolInfo>();

        foreach (var token in result.Tokens)
        {
            var tokenValues = token.Value.Split(',', ';');

            foreach (var tokenValue in tokenValues)
                symbols.Add(new SymbolInfo(tokenValue));
        }

        return symbols;
    }

    private static IReadOnlyList<PropertyInfo> ParsePropertyInfo(ArgumentResult result)
    {
        var properties = new List<PropertyInfo>();

        foreach (var token in result.Tokens)
        {
            var tokenValues = token.Value.Split(',', ';');

            foreach (var tokenValue in tokenValues)
            {
                var equalityIndex = tokenValue.IndexOf('=');
                var propertyName = tokenValue;
                var propertyValue = string.Empty;

                if (equalityIndex >= 0)
                    propertyName = tokenValue[..equalityIndex];

                if (equalityIndex > 0 && equalityIndex < tokenValue.Length - 1)
                    propertyValue = tokenValue[(equalityIndex + 1)..];

                properties.Add(new PropertyInfo(propertyName, propertyValue));
            }
        }

        return properties;
    }
}