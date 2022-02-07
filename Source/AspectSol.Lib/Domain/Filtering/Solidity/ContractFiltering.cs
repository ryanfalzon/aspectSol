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
    public SelectionResult FilterContractsByContractName(JToken jToken, string contractName)
    {
        if (string.IsNullOrWhiteSpace(contractName))
            throw new ArgumentNullException(nameof(contractName));

        var interestedContracts = new List<string>();

        if (jToken["children"] is JArray children)
        {
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(ContractDefinition) && child["kind"].Matches("contract") &&
                    (contractName.Equals(Wildcard) || child["name"].Matches(contractName)))
                {
                    interestedContracts.Add((child["name"] ?? throw new NullReferenceException()).Value<string>());
                }
            }
        }

        return new SelectionResult
        {
            InterestedContracts = interestedContracts
        };
    }

    /// <summary>
    /// Filter contracts found in jToken by the implemented interface
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="interfaceName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public SelectionResult FilterContractsByInterfaceName(JToken jToken, string interfaceName)
    {
        if (string.IsNullOrWhiteSpace(interfaceName))
            throw new ArgumentNullException(nameof(interfaceName));

        var interestedContracts = new List<string>();

        if (jToken["children"] is JArray children)
        {
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(ContractDefinition) && child["kind"].Matches("contract"))
                {
                    var baseContracts = child["baseContracts"].ToSafeList();

                    foreach (var baseContract in baseContracts)
                    {
                        if (interfaceName.Equals(Wildcard) || (baseContract["baseName"] ?? throw new NullReferenceException()).Value<JObject>()!["namePath"]
                            .Matches(interfaceName))
                        {
                            interestedContracts.Add((child["name"] ?? throw new NullReferenceException()).Value<string>());
                        }
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedContracts = interestedContracts
        };
    }
}