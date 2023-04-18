namespace unicfg.Cli;

public enum ExitCode : int
{
    Success = 0,
    NoResult = 1,
    PartialError = 2,
    Error = 3,
    UnhandledException = 26
}