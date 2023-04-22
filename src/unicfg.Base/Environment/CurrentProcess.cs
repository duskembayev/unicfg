using Enhanced.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace unicfg.Base.Environment;

[ContainerEntry(ServiceLifetime.Singleton, typeof(ICurrentProcess))]
internal sealed class CurrentProcess : ICurrentProcess
{
    public string WorkingDirectory => System.Environment.CurrentDirectory;
}
