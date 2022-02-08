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
    public SelectionResult FilterFunctionsByFunctionName(JToken jToken, string functionName)
    {
        if (string.IsNullOrWhiteSpace(functionName))
            throw new ArgumentNullException(nameof(functionName));

        var interestedFunctions = new Dictionary<string, string>();

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse() && (functionName.Equals(Wildcard) || subNode["name"].Matches(functionName)))
                    {
                        interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedFunctions = interestedFunctions
        };
    }

    /// <summary>
    /// Filter functions found in jToken by their visibility
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="visibility"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterFunctionsByVisibility(JToken jToken, string visibility)
    {
        if (string.IsNullOrWhiteSpace(visibility))
            throw new ArgumentNullException(nameof(visibility));

        var interestedFunctions = new Dictionary<string, string>();

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse() && (visibility.Equals(Wildcard) || subNode["visibility"].Matches(visibility)))
                    {
                        interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedFunctions = interestedFunctions
        };
    }

    /// <summary>
    /// Filter functions found in jToken by their state mutability
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="stateMutability"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterFunctionsByStateMutability(JToken jToken, string stateMutability)
    {
        if (string.IsNullOrWhiteSpace(stateMutability))
            throw new ArgumentNullException(nameof(stateMutability));

        var interestedFunctions = new Dictionary<string, string>();

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse() && (stateMutability.Equals(Wildcard) || subNode["stateMutability"].Matches(stateMutability)))
                    {
                        interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedFunctions = interestedFunctions
        };
    }

    /// <summary>
    /// Filter functions found in jToken by their modifier
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="modifiers"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterFunctionsByAllModifiers(JToken jToken, List<string> modifiers)
    {
        if (modifiers == null || modifiers.Count == 0)
            throw new ArgumentNullException(nameof(modifiers));

        var interestedFunctions = new Dictionary<string, string>();

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var functionModifiers = subNode["modifiers"].ToSafeList();

                        bool allMatch = true;
                        foreach (var modifier in modifiers)
                        {
                            allMatch = functionModifiers.Exists(functionModifier => functionModifier["name"].Matches(modifier));

                            if (!allMatch)
                            {
                                break;
                            }
                        }

                        if (allMatch)
                        {
                            interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                        }
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedFunctions = interestedFunctions
        };
    }

    /// <summary>
    /// Filter functions found in jToken by their modifier
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="modifiers"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterFunctionsByEitherModifiers(JToken jToken, List<string> modifiers)
    {
        if (modifiers == null || modifiers.Count == 0)
            throw new ArgumentNullException(nameof(modifiers));

        var interestedFunctions = new Dictionary<string, string>();

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var functionModifiers = subNode["modifiers"].ToSafeList();

                        bool foundMatch = false;
                        foreach (var modifier in modifiers)
                        {
                            foundMatch = functionModifiers.Exists(functionModifier => functionModifier["name"].Matches(modifier));

                            if (foundMatch)
                            {
                                break;
                            }
                        }

                        if (foundMatch)
                        {
                            interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                        }
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedFunctions = interestedFunctions
        };
    }

    /// <summary>
    /// Filter functions found in jToken by their modifier
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="modifier"></param>
    /// <param name="invert"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterFunctionsByModifier(JToken jToken, string modifier, bool invert)
    {
        if (string.IsNullOrWhiteSpace(modifier))
            throw new ArgumentNullException(nameof(modifier));

        var interestedFunctions = new Dictionary<string, string>();

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var functionModifiers = subNode["modifiers"].ToSafeList();

                        bool foundMatch = false;
                        if (functionModifiers.Count == 0 && invert)
                        {
                            foundMatch = true;
                        }
                        else
                        {
                            foreach (var functionModifier in functionModifiers)
                            {
                                foundMatch = invert ^ functionModifier["name"].Matches(modifier);

                                if (foundMatch)
                                {
                                    break;
                                }
                            }
                        }

                        if (foundMatch)
                        {
                            interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                        }
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedFunctions = interestedFunctions
        };
    }

    /// <summary>
    /// Filter functions found in jToken by the parameters they accept
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="parameterType"></param>
    /// <param name="parameterName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterFunctionsByParameters(JToken jToken, string parameterType, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(parameterType))
            throw new ArgumentNullException(nameof(parameterType));
        if (string.IsNullOrWhiteSpace(parameterName))
            throw new ArgumentNullException(nameof(parameterName));

        var interestedFunctions = new Dictionary<string, string>();

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var functionParameters = subNode["parameters"].ToSafeList();

                        bool isMatch = false;
                        foreach (var functionParameter in functionParameters)
                        {
                            if (functionParameter["type"].Matches(VariableDeclaration) &&
                                functionParameter["typeName"]["name"].Matches(parameterType) && functionParameter["identifier"]["name"].Matches(parameterName))
                            {
                                isMatch = true;
                            }
                        }

                        if (isMatch)
                        {
                            interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                        }
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedFunctions = interestedFunctions
        };
    }

    /// <summary>
    /// Filter functions found in jToken by the parameters they accept
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterFunctionsByParameters(JToken jToken, List<(string Type, string Value)> parameters)
    {
        if (parameters == null || parameters.Count == 0)
            throw new ArgumentNullException(nameof(parameters));

        var interestedFunctions = new Dictionary<string, string>();

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var functionParameters = subNode["parameters"].ToSafeList();

                        bool isMatch = functionParameters.Count > 0;
                        foreach (var functionParameter in functionParameters)
                        {
                            if (functionParameter["type"].Matches(VariableDeclaration))
                            {
                                isMatch = parameters.Exists(parameter =>
                                    functionParameter["typeName"]["name"].Matches(parameter.Type) && functionParameter["identifier"]["name"].Matches(parameter.Value));

                                if (!isMatch)
                                {
                                    break;
                                }
                            }
                        }

                        if (isMatch)
                        {
                            interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                        }
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedFunctions = interestedFunctions
        };
    }

    /// <summary>
    /// Filter functions found in JToken by the parameters the return
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="returnParameter"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterFunctionsByReturnParameters(JToken jToken, string returnParameter)
    {
        if (string.IsNullOrWhiteSpace(returnParameter))
            throw new ArgumentNullException(nameof(returnParameter));

        var interestedFunctions = new Dictionary<string, string>();

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var functionReturnParameters = subNode["returnParameters"].ToSafeList();

                        bool isMatch = false;
                        foreach (var functionReturnParameter in functionReturnParameters)
                        {
                            if (functionReturnParameter["type"].Matches(VariableDeclaration) && functionReturnParameter["typeName"]["name"].Matches(returnParameter))
                            {
                                isMatch = true;
                            }
                        }

                        if (isMatch)
                        {
                            interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                        }
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedFunctions = interestedFunctions
        };
    }

    /// <summary>
    /// Filter functions found in JToken by the parameters the return
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="returnParameters"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterFunctionsByReturnParameters(JToken jToken, List<string> returnParameters)
    {
        if (returnParameters == null || returnParameters.Count == 0)
            throw new ArgumentNullException(nameof(returnParameters));

        var interestedFunctions = new Dictionary<string, string>();

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var functionReturnParameters = subNode["returnParameters"].ToSafeList();

                        var functionReturnParametersList = new List<string>();
                        foreach (var functionReturnParameter in functionReturnParameters)
                        {
                            if (functionReturnParameter["type"].Matches(VariableDeclaration))
                            {
                                functionReturnParametersList.Add(functionReturnParameter["typeName"]["name"].Value<string>());
                            }
                        }

                        if (returnParameters.SequenceEqual(functionReturnParametersList))
                        {
                            interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                        }
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedFunctions = interestedFunctions
        };
    }

    /// <summary>
    /// Filter function statements that call a function found in an instanced contract
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="instanceName"></param>
    /// <param name="functionName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterFunctionCallsByInstanceName(JToken jToken, string instanceName, string functionName)
    {
        if (string.IsNullOrWhiteSpace(instanceName))
            throw new ArgumentNullException(nameof(instanceName));
        if (string.IsNullOrWhiteSpace(functionName))
            throw new ArgumentNullException(nameof(functionName));

        var interestedStatements = new Dictionary<(int, int, int, int?, int?), string>();

        var children = jToken["children"] as JArray;

        var childPosition = 0;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                var subNodePosition = 0;
                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var statements = subNode["body"]["statements"].ToSafeList();

                        var statementPosition = 0;
                        foreach (var statement in statements)
                        {
                            if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(FunctionCall)
                                                                               && statement["expression"]["expression"]["expression"]["name"].Matches(instanceName)
                                                                               && statement["expression"]["expression"]["memberName"].Matches(functionName))
                            {
                                interestedStatements.Add((childPosition, subNodePosition, statementPosition, null, null), subNode["name"].Value<string>());
                            }

                            statementPosition++;
                        }
                    }

                    subNodePosition++;
                }
            }

            childPosition++;
        }

        return new SelectionResult
        {
            InterestedStatements = interestedStatements
        };
    }

    /// <summary>
    /// Filter functions found in JToken based on whether they are an implementation of an interface function
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="interfaceName"></param>
    /// <param name="invert"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterFunctionsImplementedFromInterface(JToken jToken, string interfaceName, bool invert)
    {
        if (string.IsNullOrWhiteSpace(interfaceName))
            throw new ArgumentNullException(nameof(interfaceName));

        var interestedFunctions = new Dictionary<string, string>();

        var children = jToken["children"] as JArray;

        var interfaceFunctions = new List<string>();
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && child["kind"].Matches("interface") && child["name"].Matches(interfaceName))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        interfaceFunctions.Add(subNode["name"].ToString());
                    }
                }
            }
        }

        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse() &&
                        invert ^ interfaceFunctions.Contains(subNode["name"].ToString()))
                    {
                        interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                    }
                }
            }
        }

        return new SelectionResult
        {
            InterestedFunctions = interestedFunctions
        };
    }
}