using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class FunctionFiltering : IFunctionFiltering
{
    private const string Wildcard = "*";
    private const string ContractDefinition = "ContractDefinition";
    private const string FunctionDefinition = "FunctionDefinition";
    private const string VariableDeclaration = "VariableDeclaration";
    private const string ExpressionStatement = "ExpressionStatement";
    private const string FunctionCall = "FunctionCall";

    /// <summary>
    /// Filter functions found in jToken by their name
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="functionName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterFunctionsByFunctionName(JToken jToken, string functionName)
    {
        if (string.IsNullOrWhiteSpace(functionName)) throw new ArgumentNullException(nameof(functionName));

        var filteringResult = new FilteringResult();

        var children = jToken["nodes"] as JArray ?? new JArray();
        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;

            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function") &&
                        (functionName.Equals(Wildcard) || subNode["name"].Matches(functionName)))
                    {
                        var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                        filteringResult.AddFunction(contractName, currentFunctionName);
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter functions found in jToken by their visibility
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="visibility"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterFunctionsByVisibility(JToken jToken, string visibility)
    {
        if (string.IsNullOrWhiteSpace(visibility)) throw new ArgumentNullException(nameof(visibility));

        var filteringResult = new FilteringResult();

        var children = jToken["nodes"] as JArray ?? new JArray();
        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;

            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function") &&
                        (visibility.Equals(Wildcard) || subNode["visibility"].Matches(visibility)))
                    {
                        var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                        filteringResult.AddFunction(contractName, currentFunctionName);
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter functions found in jToken by their state mutability
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="stateMutability"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterFunctionsByStateMutability(JToken jToken, string stateMutability)
    {
        if (string.IsNullOrWhiteSpace(stateMutability)) throw new ArgumentNullException(nameof(stateMutability));

        var filteringResult = new FilteringResult();

        var children = jToken["nodes"] as JArray ?? new JArray();
        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;

            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function") &&
                        (stateMutability.Equals(Wildcard) || subNode["stateMutability"].Matches(stateMutability)))
                    {
                        var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                        filteringResult.AddFunction(contractName, currentFunctionName);
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter functions found in jToken by their modifier
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="modifiers"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterFunctionsByAllModifiers(JToken jToken, List<string> modifiers)
    {
        if (modifiers == null || modifiers.Count == 0) throw new ArgumentNullException(nameof(modifiers));

        var filteringResult = new FilteringResult();

        var children = jToken["nodes"] as JArray ?? new JArray();
        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;

            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function"))
                    {
                        var functionModifiers = subNode["modifiers"].ToSafeList();

                        var allMatch = true;
                        foreach (var modifier in modifiers)
                        {
                            allMatch = functionModifiers.Exists(functionModifier => functionModifier["modifierName"]["name"].Matches(modifier));

                            if (!allMatch)
                            {
                                break;
                            }
                        }

                        if (allMatch)
                        {
                            var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                            filteringResult.AddFunction(contractName, currentFunctionName);
                        }
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter functions found in jToken by their modifier
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="modifiers"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterFunctionsByEitherModifiers(JToken jToken, List<string> modifiers)
    {
        if (modifiers == null || modifiers.Count == 0) throw new ArgumentNullException(nameof(modifiers));

        var filteringResult = new FilteringResult();

        var children = jToken["nodes"] as JArray ?? new JArray();
        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;

            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function"))
                    {
                        var functionModifiers = subNode["modifiers"].ToSafeList();

                        var foundMatch = false;
                        foreach (var modifier in modifiers)
                        {
                            foundMatch = functionModifiers.Exists(functionModifier => functionModifier["modifierName"]["name"].Matches(modifier));

                            if (foundMatch)
                            {
                                break;
                            }
                        }

                        if (foundMatch)
                        {
                            var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                            filteringResult.AddFunction(contractName, currentFunctionName);
                        }
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter functions found in jToken by their modifier
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="modifier"></param>
    /// <param name="invert"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterFunctionsByModifier(JToken jToken, string modifier, bool invert)
    {
        if (string.IsNullOrWhiteSpace(modifier)) throw new ArgumentNullException(nameof(modifier));

        var filteringResult = new FilteringResult();

        var children = jToken["nodes"] as JArray ?? new JArray();
        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;

            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function"))
                    {
                        var functionModifiers = subNode["modifiers"].ToSafeList();

                        var foundMatch = false;
                        if (functionModifiers.Count == 0 && invert)
                        {
                            foundMatch = true;
                        }
                        else
                        {
                            foreach (var functionModifier in functionModifiers)
                            {
                                foundMatch = invert ^ functionModifier["modifierName"]?["name"].Matches(modifier) ?? false;

                                if (foundMatch)
                                {
                                    break;
                                }
                            }
                        }

                        if (foundMatch)
                        {
                            var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                            filteringResult.AddFunction(contractName, currentFunctionName);
                        }
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter functions found in jToken by the parameters they accept
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="parameterType"></param>
    /// <param name="parameterName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterFunctionsByParameters(JToken jToken, string parameterType, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(parameterType)) throw new ArgumentNullException(nameof(parameterType));
        if (string.IsNullOrWhiteSpace(parameterName)) throw new ArgumentNullException(nameof(parameterName));

        var filteringResult = new FilteringResult();

        var children = jToken["nodes"] as JArray ?? new JArray();
        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;

            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function"))
                    {
                        var functionParameters = subNode["parameters"]?["parameters"].ToSafeList() ?? new List<JToken>();

                        var isMatch = false;
                        foreach (var functionParameter in functionParameters)
                        {
                            if (functionParameter["nodeType"].Matches(VariableDeclaration) &&
                                (functionParameter["typeName"]?["name"].Matches(parameterType) ?? false) && functionParameter["name"].Matches(parameterName))
                            {
                                isMatch = true;
                            }
                        }

                        if (isMatch)
                        {
                            var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                            filteringResult.AddFunction(contractName, currentFunctionName);
                        }
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter functions found in jToken by the parameters they accept
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterFunctionsByParameters(JToken jToken, List<(string Type, string Value)> parameters)
    {
        if (parameters == null || parameters.Count == 0) throw new ArgumentNullException(nameof(parameters));

        var filteringResult = new FilteringResult();

        var children = jToken["nodes"] as JArray ?? new JArray();
        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;

            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function"))
                    {
                        var functionParameters = subNode["parameters"]?["parameters"].ToSafeList() ?? new List<JToken>();

                        var isMatch = functionParameters.Count > 0;
                        foreach (var functionParameter in functionParameters)
                        {
                            if (functionParameter["nodeType"].Matches(VariableDeclaration))
                            {
                                isMatch = parameters.Exists(parameter =>
                                    (functionParameter["typeName"]?["name"].Matches(parameter.Type) ?? false) &&
                                    functionParameter["name"].Matches(parameter.Value));

                                if (!isMatch)
                                {
                                    break;
                                }
                            }
                        }

                        if (isMatch)
                        {
                            var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                            filteringResult.AddFunction(contractName, currentFunctionName);
                        }
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter functions found in JToken by the parameters the return
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="returnParameter"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterFunctionsByReturnParameters(JToken jToken, string returnParameter)
    {
        if (string.IsNullOrWhiteSpace(returnParameter)) throw new ArgumentNullException(nameof(returnParameter));

        var filteringResult = new FilteringResult();

        var children = jToken["nodes"] as JArray ?? new JArray();
        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;

            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function"))
                    {
                        var functionReturnParameters = subNode["returnParameters"]?["parameters"].ToSafeList() ?? new List<JToken>();

                        var isMatch = false;
                        foreach (var functionReturnParameter in functionReturnParameters)
                        {
                            if (functionReturnParameter["nodeType"].Matches(VariableDeclaration) &&
                                (functionReturnParameter["typeName"]?["name"].Matches(returnParameter) ?? false))
                            {
                                isMatch = true;
                            }
                        }

                        if (isMatch)
                        {
                            var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                            filteringResult.AddFunction(contractName, currentFunctionName);
                        }
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter functions found in JToken by the parameters the return
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="returnParameters"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterFunctionsByReturnParameters(JToken jToken, List<string> returnParameters)
    {
        if (returnParameters == null || returnParameters.Count == 0) throw new ArgumentNullException(nameof(returnParameters));

        var filteringResult = new FilteringResult();

        var children = jToken["nodes"] as JArray ?? new JArray();
        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;

            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function"))
                    {
                        var functionReturnParameters = subNode["returnParameters"]?["parameters"].ToSafeList() ?? new List<JToken>();

                        var functionReturnParametersList = new List<string>();
                        foreach (var functionReturnParameter in functionReturnParameters)
                        {
                            if (functionReturnParameter["nodeType"].Matches(VariableDeclaration))
                            {
                                functionReturnParametersList.Add(functionReturnParameter["typeName"]?["name"]?.Value<string>());
                            }
                        }

                        if (returnParameters.SequenceEqual(functionReturnParametersList))
                        {
                            var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                            filteringResult.AddFunction(contractName, currentFunctionName);
                        }
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter function statements that call a function found in an instanced contract
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="instanceName"></param>
    /// <param name="functionName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterFunctionCallsByInstanceName(JToken jToken, string instanceName, string functionName)
    {
        if (string.IsNullOrWhiteSpace(instanceName)) throw new ArgumentNullException(nameof(instanceName));
        if (string.IsNullOrWhiteSpace(functionName)) throw new ArgumentNullException(nameof(functionName));

        var filteringResult = new FilteringResult();

        var children = jToken["nodes"] as JArray ?? new JArray();

        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;

            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;

                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function"))
                    {
                        var statements = subNode["body"]?["statements"].ToSafeList() ?? new List<JToken>();

                        var statementPosition = 0;
                        foreach (var statement in statements)
                        {
                            if (statement["nodeType"].Matches(ExpressionStatement) && (statement["expression"]?["nodeType"].Matches(FunctionCall) ?? false) &&
                                (statement["expression"]?["expression"]?["expression"]?["name"].Matches(instanceName) ?? false) &&
                                statement["expression"]["expression"]["memberName"].Matches(functionName))
                            {
                                filteringResult.AddStatement(contractName, currentFunctionName, statementPosition);
                            }

                            statementPosition++;
                        }
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter functions found in JToken based on whether they are an implementation of an interface function
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="interfaceName"></param>
    /// <param name="invert"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterFunctionsImplementedFromInterface(JToken jToken, string interfaceName, bool invert)
    {
        if (string.IsNullOrWhiteSpace(interfaceName)) throw new ArgumentNullException(nameof(interfaceName));

        var filteringResult = new FilteringResult();

        var children = jToken["nodes"] as JArray ?? new JArray();

        var interfaceFunctions = new List<string>();
        foreach (var child in children.Children())
        {
            if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("interface") && child["name"].Matches(interfaceName))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function"))
                    {
                        var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                        interfaceFunctions.Add(currentFunctionName);
                    }
                }
            }
        }

        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;

            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function") &&
                        invert ^ interfaceFunctions.Contains(subNode["name"]?.Value<string>()))
                    {
                        var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                        filteringResult.AddFunction(contractName, currentFunctionName);
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter function statements that call a function found in an instanced contract
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="functionName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterFunctionCalls(JToken jToken, string functionName)
    {
        if (string.IsNullOrWhiteSpace(functionName)) throw new ArgumentNullException(nameof(functionName));

        var filteringResult = new FilteringResult();

        var children = jToken["nodes"] as JArray ?? new JArray();

        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;

            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;

                    if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function"))
                    {
                        var statements = subNode["body"]?["statements"].ToSafeList() ?? new List<JToken>();

                        var statementPosition = 0;
                        foreach (var statement in statements)
                        {
                            if (statement["nodeType"].Matches(ExpressionStatement) && (statement["expression"]?["nodeType"].Matches(FunctionCall) ?? false) &&
                                (statement["expression"]?["expression"]?["memberName"].Matches(functionName) ?? false))
                            {
                                filteringResult.AddStatement(contractName, currentFunctionName, statementPosition);
                            }

                            statementPosition++;
                        }
                    }
                }
            }
        }

        return filteringResult;
    }
}