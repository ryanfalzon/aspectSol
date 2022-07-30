using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;

namespace AspectSol.Lib.Infra.Comparers;

public class DefinitionFilteringResultComparer : IEqualityComparer<DefinitionFilteringResult>
{
    public bool Equals(DefinitionFilteringResult x, DefinitionFilteringResult y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.DefinitionName == y.DefinitionName;
    }

    public int GetHashCode(DefinitionFilteringResult obj)
    {
        return obj.DefinitionName != null ? obj.DefinitionName.GetHashCode() : 0;
    }
}