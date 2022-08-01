using AspectSol.Lib.Domain.Filtering.FilteringResults;

namespace AspectSol.Lib.Infra.Comparers;

public class FunctionFilteringResultComparer : IEqualityComparer<FunctionFilteringResult>
{
    public bool Equals(FunctionFilteringResult x, FunctionFilteringResult y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.FunctionName == y.FunctionName;
    }

    public int GetHashCode(FunctionFilteringResult obj)
    {
        return obj.FunctionName != null ? obj.FunctionName.GetHashCode() : 0;
    }
}