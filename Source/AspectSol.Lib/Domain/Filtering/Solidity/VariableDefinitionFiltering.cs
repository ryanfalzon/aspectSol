using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class VariableDefinitionFiltering : IVariableDefinitionFiltering
{
    private const string Wildcard = "*";
    private const string ContractDefinition = "ContractDefinition";
    private const string VariableDeclaration = "VariableDeclaration";
    private const string StateVariableDeclaration = "VariableDeclaration";

    /// <summary>
    /// Filter variable definitions found in jToken by their type
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="variableType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterVariableDefinitionByVariableType(JToken jToken, string variableType)
    {
        if (string.IsNullOrWhiteSpace(variableType))
            throw new ArgumentNullException(nameof(variableType));

        var interestedDefinitions = new Dictionary<string, string>();

        if (jToken["nodes"] is JArray children)
        {
            foreach (var child in children.Children())
            {
                if (child["nodeType"].Matches(ContractDefinition))
                {
                    var subNodes = child["nodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["nodeType"].Matches(VariableDeclaration) &&
                            (variableType.Equals(Wildcard) || subNode["typeDescriptions"]["typeString"].Matches(variableType)))
                        {
                            interestedDefinitions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                        }
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedDefinitions = interestedDefinitions
        };
    }

    /// <summary>
    /// Filter variable definitions found in jToken by their name
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="variableName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterVariableDefinitionByVariableName(JToken jToken, string variableName)
    {
        if (string.IsNullOrWhiteSpace(variableName))
            throw new ArgumentNullException(nameof(variableName));

        var interestedDefinitions = new Dictionary<string, string>();

        if (jToken["nodes"] is JArray children)
        {
            foreach (var child in children.Children())
            {
                if (child["nodeType"].Matches(ContractDefinition))
                {
                    var subNodes = child["nodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["nodeType"].Matches(VariableDeclaration) &&
                            (variableName.Equals(Wildcard) || subNode["name"].Matches(variableName)))
                        {
                            interestedDefinitions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                        }
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedDefinitions = interestedDefinitions
        };
    }
}