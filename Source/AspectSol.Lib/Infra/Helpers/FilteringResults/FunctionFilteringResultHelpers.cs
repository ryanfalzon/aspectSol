using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Comparers;

namespace AspectSol.Lib.Infra.Helpers.FilteringResults;

public static class FunctionFilteringResultHelpers
{
    public static IEnumerable<FunctionFilteringResult> Intersect(IReadOnlyCollection<FunctionFilteringResult[]> filteringResults)
    {
        var intersection = filteringResults.Aggregate<IEnumerable<FunctionFilteringResult>>(
            (previousList, nextList) => previousList.Intersect(nextList, new FunctionFilteringResultComparer())
        ).ToList();
        
        foreach (var intersectionItem in intersection)
        {
            var intersectedItemsChildren = filteringResults
                .SelectMany(x => x.Where(y => y.FunctionName.Equals(intersectionItem.FunctionName)))
                .Where(x => x != null)
                .Select(x => x.StatementFilteringResults)
                .Where(x => x != null)
                .Select(x => x.ToArray()).ToList();
            
            intersectionItem.ReplaceStatementFilteringResults(StatementFilteringResultHelpers.Intersect(intersectedItemsChildren).ToList());
        }

        return intersection;
    }

    public static FilteringResult Collate(FilteringResult primary, FilteringResult secondary)
    {
        var collation = new FilteringResult();

        foreach (var primaryContract in primary.ContractFilteringResults)
        {
            foreach (var primaryFunction in primaryContract.FunctionFilteringResults)
            {
                if (secondary.ContractFilteringResults.Exists(x => x.ContractName.Equals(primaryContract.ContractName)))
                {
                    collation.AddFunction(primaryContract.ContractName, primaryFunction.FunctionName);
                }
            }
        }

        return collation;
    }
}