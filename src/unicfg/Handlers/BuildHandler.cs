using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using Microsoft.Extensions.Logging;
using unicfg.Cli;
using unicfg.Evaluation;

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
        var inputs = context.ParseResult.GetValueForArgument(CliSymbols.InputsArgument);
        var properties = context.ParseResult.GetValueForOption(CliSymbols.PropertiesOption);
        var outputDir = context.ParseResult.GetValueForOption(CliSymbols.OutputDirOption);

        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentNullException.ThrowIfNull(outputDir);

        try
        {
            var propertiesCount = properties.Count;

            string outputDirName = outputDir.Name;

            _logger.LogInformation("test");
            _logger.LogError("test");
            _logger.LogDebug("test");
            _logger.LogTrace("test");
            _logger.LogWarning("test");
            _logger.LogCritical("test");
            return Task.FromResult(0);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}