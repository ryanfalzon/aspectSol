using AspectSol.Lib.Domain.Filtering.FilteringResults;

namespace AspectSol.Lib.Infra.Comparers;

public class StatementFilteringResultComparer : IEqualityComparer<StatementFilteringResult>
{
    public bool Equals(StatementFilteringResult x, StatementFilteringResult y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.StatementIndex == y.StatementIndex;
    }

    public int GetHashCode(StatementFilteringResult obj)
    {
        return obj.StatementIndex;
    }
}