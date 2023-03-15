using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace unicfg.Cli;

internal static class Verbosity
{
    internal static readonly Option<Level> Option = CreateVerbosityOption();

    private static Option<Level> CreateVerbosityOption()
    {
        var verbosityOption = new Option<Level>(new[] { "-v", "--verbosity" }, () => Level.Normal)
        {
            Arity = ArgumentArity.ZeroOrOne,
            Description =
                "Set the verbosity level. Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].",
            ArgumentHelpName = "LEVEL"
        };

        verbosityOption.AddValidator(result =>
        {
            var value = result.GetValueOrDefault<string>();

            if (value is null)
                return;

            var levels = Enum.GetNames<Level>();

            if (!levels.Contains(value, StringComparer.OrdinalIgnoreCase))
                result.ErrorMessage = "Invalid verbosity level";
        });
        return verbosityOption;
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