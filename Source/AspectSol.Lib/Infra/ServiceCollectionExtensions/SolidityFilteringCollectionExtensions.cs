using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.Solidity;
using AspectSol.Lib.Infra.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace AspectSol.Lib.Infra.ServiceCollectionExtensions;

public static class SolidityFilteringCollectionExtensions
{
    public static void AddSolidityFilteringConfig(this IServiceCollection services)
    {
        services.AddTransient<ContractFiltering>();
        services.AddTransient<FunctionFiltering>();
        services.AddTransient<SourceManipulation>();
        services.AddTransient<VariableDefinitionFiltering>();
        services.AddTransient<VariableGettersFiltering>();
        services.AddTransient<VariableSettersFiltering>();
        
        services.AddTransient<AbstractFilteringService, SolidityFilteringService>();

        services.AddSingleton<SolidityAstNodeIdResolver>();
    }
}