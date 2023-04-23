namespace unicfg.Cli;

internal enum ExitCode
{
    Success = 0,
    NoResult = 1,
    PartialError = 2,
    Error = 3,
    UnhandledException = 26
}
