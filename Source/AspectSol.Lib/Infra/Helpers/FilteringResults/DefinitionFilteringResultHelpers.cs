using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Comparers;

namespace AspectSol.Lib.Infra.Helpers.FilteringResults;

public static class DefinitionFilteringResultHelpers
{
    public static IEnumerable<DefinitionFilteringResult> Intersect(IEnumerable<DefinitionFilteringResult[]> filteringResults)
    {
        var intersection = filteringResults.Aggregate<IEnumerable<DefinitionFilteringResult>>(
            (previousList, nextList) => previousList.Intersect(nextList, new DefinitionFilteringResultComparer())
        ).ToList();

        return intersection;
    }
}