using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Comparers;

namespace AspectSol.Lib.Infra.Helpers.FilteringResults;

public static class StatementFilteringResultHelpers
{
    public static IEnumerable<StatementFilteringResult> Intersect(IReadOnlyCollection<StatementFilteringResult[]> filteringResults)
    {
        var intersection = filteringResults.Aggregate<IEnumerable<StatementFilteringResult>>(
            (previousList, nextList) => previousList.Intersect(nextList, new StatementFilteringResultComparer())
        ).ToList();

        foreach (var intersectionItem in intersection)
        {
            var intersectedItemsChildren = filteringResults
                .Where(x => x != null)
                .SelectMany(x => x.Where(y => y.StatementIndex.Equals(intersectionItem.StatementIndex)))
                .Select(x => x.StatementFilteringResults)
                .Where(x => x != null)
                .Select(x => x.ToArray()).ToList();

            intersectionItem.ReplaceStatementFilteringResults(Intersect(intersectedItemsChildren).ToList());
        }

        return intersection;
    }
}