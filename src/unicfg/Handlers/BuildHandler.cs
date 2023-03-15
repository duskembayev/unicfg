using System.CommandLine.Invocation;
using Microsoft.Extensions.Logging;

namespace unicfg.Handlers;

internal class BuildHandler : ICommandHandler
{
    private readonly ILogger<BuildHandler> _logger;

    public BuildHandler(ILogger<BuildHandler> logger)
    {
        _logger = logger;
    }
    
    public int Invoke(InvocationContext context)
    {
        throw new NotSupportedException();
    }

    public Task<int> InvokeAsync(InvocationContext context)
    {
        _logger.LogInformation("test");
        _logger.LogError("test");
        _logger.LogDebug("test");
        _logger.LogTrace("test");
        _logger.LogWarning("test");
        _logger.LogCritical("test");
        return Task.FromResult(0);
    }
}