using AspectSol.Lib.App;
using AspectSol.Lib.Domain.Interpreter;
using AspectSol.Lib.Domain.JavascriptExecution;
using AspectSol.Lib.Domain.Parser;
using AspectSol.Lib.Domain.Tokenizer;
using AspectSol.Lib.Infra.TemporaryStorage;
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
        services.AddScoped<IInterpreter, Interpreter>();

        services.AddNodeJS();
        services.AddScoped<ScriptFactory>();
        services.AddScoped<TempStorageRepository>();
        services.AddScoped<IJavascriptExecutor, JavascriptExecutor>();

        services.AddSolidityFilteringConfig();
    }
}