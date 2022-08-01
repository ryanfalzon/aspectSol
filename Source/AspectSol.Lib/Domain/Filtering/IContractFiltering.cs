using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface IContractFiltering
{
    FilteringResult FilterContractsByContractName(JToken jToken, string contractName);
    FilteringResult FilterContractsByInterfaceName(JToken jToken, string interfaceName);
}