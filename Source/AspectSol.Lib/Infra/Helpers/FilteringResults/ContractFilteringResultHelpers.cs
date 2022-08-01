using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Comparers;

namespace AspectSol.Lib.Infra.Helpers.FilteringResults;

public static class ContractFilteringResultHelpers
{
    public static IEnumerable<ContractFilteringResult> Intersect(IReadOnlyCollection<ContractFilteringResult[]> filteringResults)
    {
        var intersection = filteringResults.Aggregate<IEnumerable<ContractFilteringResult>>(
            (previousList, nextList) => previousList.Intersect(nextList, new ContractFilteringResultComparer())
        ).ToList();
        
        foreach (var intersectionItem in intersection)
        {
            var intersectedItemsFunctionChildren = filteringResults
                .Where(x => x != null)
                .SelectMany(x => x.Where(y => y.ContractName.Equals(intersectionItem.ContractName)))
                .Select(x => x.FunctionFilteringResults)
                .Where(x => x != null)
                .Select(x => x.ToArray()).ToList();
            
            intersectionItem.ReplaceFunctionFilteringResults(FunctionFilteringResultHelpers.Intersect(intersectedItemsFunctionChildren).ToList());
            
            var intersectedItemsDefinitionChildren = filteringResults
                .Where(x => x != null)
                .SelectMany(x => x.Where(y => y.ContractName.Equals(intersectionItem.ContractName)))
                .Select(x => x.DefinitionFilteringResults)
                .Where(x => x != null)
                .Select(x => x.ToArray()).ToList();
            
            intersectionItem.ReplaceDefinitionFilteringResults(DefinitionFilteringResultHelpers.Intersect(intersectedItemsDefinitionChildren).ToList());
        }

        return intersection;
    }
}