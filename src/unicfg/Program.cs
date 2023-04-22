using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using unicfg.Build;
using unicfg.Cli;
using unicfg.Enhanced.DependencyInjection;
using unicfg.Eval;

var rootCommand = new RootCommand("unicfg CLI")
{
    new BuildCommand(),
    new EvalCommand()
};

rootCommand.AddGlobalOption(Verbosity.Option);

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
    var invocationContext = builder.GetInvocationContext();

    if (invocationContext.ParseResult.Errors.Count > 0)
    {
        return;
    }

    builder.ConfigureLoggingByVerbosity();
    builder.ConfigureServices(
        collection =>
        {
            collection.Configure<InvocationLifetimeOptions>(options => options.SuppressStatusMessages = true);
            collection.AddEnhancedModules();
        });

    builder.UseCommandHandler<BuildCommand, BuildHandler>();
    builder.UseCommandHandler<EvalCommand, EvalHandler>();
}
