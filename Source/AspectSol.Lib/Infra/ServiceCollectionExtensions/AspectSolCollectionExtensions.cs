using AspectSol.Lib.App;
using AspectSol.Lib.Domain.JavascriptExecution;
using AspectSol.Lib.Domain.Parsing;
using AspectSol.Lib.Domain.Tokenization;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;

namespace AspectSol.Lib.Infra.ServiceCollectionExtensions;

public static class AspectSolCollectionExtensions
{
    public static void AddAspectSolServiceConfig(this IServiceCollection services)
    {
        services.AddTransient<AppService>();

        services.AddScoped<ITokenizer, Tokenizer>();
        services.AddScoped<IParser, Parser>();

        services.AddNodeJS();
        services.AddScoped<ScriptFactory>();
        services.AddScoped<IJavascriptExecutor, JavascriptExecutor>();

        services.AddSolidityFilteringConfig();
    }
}