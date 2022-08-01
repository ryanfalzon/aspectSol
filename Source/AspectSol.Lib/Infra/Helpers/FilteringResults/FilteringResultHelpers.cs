using AspectSol.Lib.Domain.Filtering.FilteringResults;

namespace AspectSol.Lib.Infra.Helpers.FilteringResults;

public class FilteringResultHelpers
{
    public static FilteringResult Intersect(params FilteringResult[] filteringResults)
    {
        var intersectedItemsChildren = filteringResults
            .Where(x => x != null)
            .Select(x => x.ContractFilteringResults)
            .Where(x => x != null)
            .Select(x => x.ToArray()).ToList();
        
        FilteringResult filteringResult = new FilteringResult();
        filteringResult.ReplaceContractFilteringResults(ContractFilteringResultHelpers.Intersect(intersectedItemsChildren).ToList());

        return filteringResult;
    }
    
    public static FilteringResult Union(params FilteringResult[] filteringResults)
    {
        throw new NotImplementedException("OR operator not yet supported by interpreter");
    }
}