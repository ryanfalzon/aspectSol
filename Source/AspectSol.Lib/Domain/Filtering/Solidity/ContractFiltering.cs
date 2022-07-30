using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class ContractFiltering : IContractFiltering
{
    private const string Wildcard = "*";
    private const string ContractDefinition = "ContractDefinition";

    /// <summary>
    /// Filter contracts found in jToken by their name
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="contractName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public FilteringResult FilterContractsByContractName(JToken jToken, string contractName)
    {
        if (string.IsNullOrWhiteSpace(contractName))
            throw new ArgumentNullException(nameof(contractName));

        var filteringResult = new FilteringResult();

        if (jToken["nodes"] is JArray children)
        {
            foreach (var child in children.Children())
            {
                if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") &&
                    (contractName.Equals(Wildcard) || child["name"].Matches(contractName)))
                {
                    filteringResult.AddContract(child["name"]?.Value<string>());
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter contracts found in jToken by the implemented interface
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="interfaceName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public FilteringResult FilterContractsByInterfaceName(JToken jToken, string interfaceName)
    {
        if (string.IsNullOrWhiteSpace(interfaceName))
            throw new ArgumentNullException(nameof(interfaceName));

        var filteringResult = new FilteringResult();

        if (jToken["nodes"] is JArray children)
        {
            foreach (var child in children.Children())
            {
                if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract"))
                {
                    var baseContracts = child["baseContracts"].ToSafeList();

                    foreach (var baseContract in baseContracts)
                    {
                        if (interfaceName.Equals(Wildcard) || (baseContract["baseName"] ?? throw new NullReferenceException()).Value<JObject>()!["name"]
                            .Matches(interfaceName))
                        {
                            filteringResult.AddContract(child["name"]?.Value<string>());
                        }
                    }
                }
            }
        }

        return filteringResult;
    }
}