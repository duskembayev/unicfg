namespace unicfg.Base.Environment;

public class CurrentProcess : ICurrentProcess
{
    public string WorkingDirectory => System.Environment.CurrentDirectory;
}