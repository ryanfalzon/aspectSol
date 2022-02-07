using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class VariableDefinitionFiltering : IVariableDefinitionFiltering
{
    private const string Wildcard = "*";
    private const string ContractDefinition = "ContractDefinition";
    private const string VariableDeclaration = "VariableDeclaration";
    private const string StateVariableDeclaration = "StateVariableDeclaration";

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

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(StateVariableDeclaration))
                    {
                        var variables = subNode["variables"].ToSafeList();
                        foreach (var variable in variables)
                        {
                            if (variable["type"].Matches(VariableDeclaration) && (variableType.Equals(Wildcard) || variable["typeName"]["name"].Matches(variableType)))
                            {
                                interestedDefinitions.Add(variable["name"].Value<string>(), child["name"].Value<string>());
                            }
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

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(StateVariableDeclaration))
                    {
                        var variables = subNode["variables"].ToSafeList();
                        foreach (var variable in variables)
                        {
                            if (variable["type"].Matches(VariableDeclaration) && (variableName.Equals(Wildcard) || variable["name"].Matches(variableName)))
                            {
                                interestedDefinitions.Add(variable["name"].Value<string>(), child["name"].Value<string>());
                            }
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