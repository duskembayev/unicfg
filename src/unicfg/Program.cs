using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Hosting;
using unicfg.Cli;
using unicfg.Enhanced.DependencyInjection;

var rootCommand = Commands.RootCommand();

return await new CommandLineBuilder(rootCommand)
    .UseHelp()
    .UseVersionOption()
    .UseTypoCorrections()
    .UseParseErrorReporting()
    .UseExceptionHandler()
    .CancelOnProcessTermination()
    .UseHost(ConfigureHost)
    .Build()
    .InvokeAsync(args);

static void ConfigureHost(IHostBuilder builder)
{
    builder.ConfigureServices(collection => collection.AddEnhancedModules());
}