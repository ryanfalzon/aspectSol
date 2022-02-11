using AspectSol.Lib.App;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.Solidity;
using AspectSol.Lib.Domain.Parsing;
using AspectSol.Lib.Domain.Tokenization;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;

namespace AspectSol.Lib.Infra;

public static class AspectSolCollectionExtensions
{
    public static void AddAspectSolServiceConfig(this IServiceCollection services)
    {
        services.AddTransient<AppService>();
        services.AddTransient<SolidityFilteringService>();

        services.AddScoped<ITokenizer, Tokenizer>();
        services.AddScoped<IParser, Parser>();

        services.AddNodeJS();
        services.Configure<NodeJSProcessOptions>(options => options.ProjectPath = AppDomain.CurrentDomain.BaseDirectory);

        services.AddScoped<IContractFiltering, ContractFiltering>();
        services.AddScoped<IFunctionFiltering, FunctionFiltering>();
        services.AddScoped<INodeGenerator, NodeGenerator>();
        services.AddScoped<ISourceManipulation, SourceManipulation>();
        services.AddScoped<IVariableDefinitionFiltering, VariableDefinitionFiltering>();
        services.AddTransient<VariableGettersFiltering>();
        services.AddTransient<VariableSettersFiltering>();
    }
}