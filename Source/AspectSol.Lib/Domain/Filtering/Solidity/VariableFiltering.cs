using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public abstract class VariableFiltering
{
    protected const string ContractDefinition = "ContractDefinition";
    protected const string StateVariableDeclaration = "StateVariableDeclaration";

    protected readonly Dictionary<(string ContractName, string VariableName), (string VariableType, string VariableVisibility)> Locals;

    protected VariableFiltering()
    {
        Locals = new Dictionary<(string, string), (string, string)>();
    }

    public void LoadLocals(JToken source)
    {
        var children = source["children"] as JArray;

        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var contractName = child["name"].ToString();
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(StateVariableDeclaration))
                    {
                        var variables = subNode["variables"].ToSafeList();

                        foreach (var variable in variables)
                        {
                            var variableName = variable["name"].ToString();
                            var variableType = variable["typeName"]["name"] == null ? variable["typeName"]["namePath"].ToString() : variable["typeName"]["name"].ToString();
                            var variableVisibility = variable["visibility"].ToString();

                            Locals.Add((contractName, variableName), (variableType, variableVisibility));
                        }
                    }
                }
            }
        }
    }

    protected bool IsVariableTypeMatch(string contractName, string variableName, string variableType)
    {
        return Locals.ContainsKey((contractName, variableName)) &&
               Locals[(contractName, variableName)].VariableType == variableType;
    }

    protected bool IsVariableTypeMatch(string contractName, JToken variableName, string variableType)
    {
        return variableName?.Value<string>() != null &&
               IsVariableTypeMatch(contractName, variableName.Value<string>(), variableType);
    }

    protected bool IsVariableVisibilityMatch(string contractName, string variableName, string variableVisibility)
    {
        return Locals.ContainsKey((contractName, variableName)) && (
               Locals[(contractName, variableName)].VariableVisibility == "default" && variableVisibility == "internal" ||
               Locals[(contractName, variableName)].VariableVisibility == variableVisibility);
    }

    protected bool IsVariableVisibilityMatch(string contractName, JToken variableName, string variableVisibility)
    {
        return variableName?.Value<string>() != null &&
               IsVariableVisibilityMatch(contractName, variableName.Value<string>(), variableVisibility);
    }
}