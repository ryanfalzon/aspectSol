using AspectSol.Lib.Domain.Filtering.FilteringResults;

namespace AspectSol.Lib.Infra.Comparers;

public class ContractFilteringResultComparer : IEqualityComparer<ContractFilteringResult>
{
    public bool Equals(ContractFilteringResult x, ContractFilteringResult y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.ContractName == y.ContractName;
    }

    public int GetHashCode(ContractFilteringResult obj)
    {
        return obj.ContractName != null ? obj.ContractName.GetHashCode() : 0;
    }
}