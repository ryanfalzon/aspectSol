using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface IContractFiltering
{
    SelectionResult FilterContractsByContractName(JToken jToken, string contractName);
    SelectionResult FilterContractsByInterfaceName(JToken jToken, string interfaceName);
}