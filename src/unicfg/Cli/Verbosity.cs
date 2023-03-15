using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;

namespace unicfg.Cli;

internal static class Verbosity
{
    internal static readonly Option<Level> Option = CreateVerbosityOption();

    private static Option<Level> CreateVerbosityOption()
    {
        var verbosityOption = new Option<Level>(new[] {"-v", "--verbosity"}, ParseLevel, isDefault: true)
        {
            Arity = ArgumentArity.ZeroOrOne,
            Description =
                "Set the verbosity level. Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].",
            ArgumentHelpName = "LEVEL"
        };

        verbosityOption.AddValidator(result =>
        {
            if (result.Tokens.Count == 0)
                return;

            var value = result.Tokens.Single().Value;
            var levels = Enum.GetNames<Level>();

            if (!levels.Contains(value, StringComparer.OrdinalIgnoreCase))
                result.ErrorMessage = "Invalid verbosity level";
        });
        return verbosityOption;
    }

    private static Level ParseLevel(ArgumentResult result)
    {
        if (result.Parent is OptionResult { IsImplicit: true })
            return Level.Normal;

        if (result.Tokens.Count == 0)
            return Level.Diag;

        var value = result.Tokens.Single().Value;

        if (string.IsNullOrEmpty(value))
            return Level.Detailed;

        return Enum.Parse<Level>(value, true);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal enum Level
    {
        Quiet,
        Minimal,
        Normal,
        Detailed,
        Diagnostic,
        Q = Quiet,
        M = Minimal,
        N = Normal,
        D = Detailed,
        Diag = Diagnostic
    }
}