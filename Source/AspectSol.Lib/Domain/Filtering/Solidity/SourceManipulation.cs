using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class SourceManipulation : ISourceManipulation
{
    private const string ContractDefinition = "ContractDefinition";
    private const string FunctionDefinition = "FunctionDefinition";
    
    public void AddContract(ref JToken source, JToken contract)
    {
        var sourceArray = source["nodes"] as JArray;
        sourceArray?.AddFirst(contract);
    }

    public void AddContracts(ref JToken source, List<JToken> contracts)
    {
        foreach (var contract in contracts)
        {
            AddContract(ref source, contract);
        }
    }

    public void AddInterfaceToContract(ref JToken source, JToken contractInterface, SelectionResult selectionResult)
    {
        foreach (var interestedContract in selectionResult.InterestedContracts)
        {
            if (source["nodes"] is JArray nodes)
            {
                foreach (var child in nodes.Children())
                {
                    if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") && child["name"].Matches(interestedContract))
                    {
                        var baseContracts = child["baseContracts"] as JArray;
                        baseContracts?.AddFirst(contractInterface);
                    }
                }
            }
        }
    }

    public void AddDefinitionToContract(ref JToken source, JToken definition, SelectionResult selectionResult)
    {
        foreach (var interestedContract in selectionResult.InterestedContracts)
        {
            if (source["nodes"] is JArray nodes)
            {
                foreach (var child in nodes.Children())
                {
                    if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") && child["name"].Matches(interestedContract))
                    {
                        var baseContracts = child["nodes"] as JArray;
                        baseContracts?.AddFirst(definition);
                    }
                }
            }
        }
    }

    public void AddDefinitionsToContract(ref JToken source, List<JToken> definitions, SelectionResult selectionResult)
    {
        foreach (var definition in definitions)
        {
            AddDefinitionToContract(ref source, definition, selectionResult);
        }
    }

    public void AddDefinitionToFunction(ref JToken source, JToken definition, SelectionResult selectionResult)
    {
        foreach (var interestedFunctions in selectionResult.InterestedFunctions)
        {
            if (source["nodes"] is JArray nodes)
            {
                foreach (var child in nodes.Children())
                {
                    if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") &&
                        child["name"].Matches(interestedFunctions.Value))
                    {
                        var subNodes = child["nodes"].ToSafeList();

                        foreach (var subNode in subNodes)
                        {
                            if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function") &&
                                subNode["name"].Matches(interestedFunctions.Key))
                            {
                                var statements = subNode["body"]["statements"] as JArray;
                                statements?.AddFirst(definition);
                            }
                        }
                    }
                }
            }
        }
    }

    public void AddDefinitionsToFunction(ref JToken source, List<JToken> definitions, SelectionResult selectionResult)
    {
        foreach (var definition in definitions)
        {
            AddDefinitionToFunction(ref source, definition, selectionResult);
        }
    }

    public void UpdateContractName(ref JToken source, SelectionResult selectionResult, string name)
    {
        foreach (var interestedContract in selectionResult.InterestedContracts)
        {
            if (source["nodes"] is JArray nodes)
            {
                foreach (var child in nodes.Children())
                {
                    if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") && child["name"].Matches(interestedContract))
                    {
                        child["name"] = name;
                    }
                }
            }
        }
    }

    public void UpdateFunctionName(ref JToken source, SelectionResult selectionResult, string name)
    {
        foreach (var interestedFunctions in selectionResult.InterestedFunctions)
        {
            if (source["nodes"] is JArray nodes)
            {
                foreach (var child in nodes.Children())
                {
                    if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") &&
                        child["name"].Matches(interestedFunctions.Value))
                    {
                        var subNodes = child["nodes"].ToSafeList();

                        foreach (var subNode in subNodes)
                        {
                            if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function") &&
                                subNode["name"].Matches(interestedFunctions.Key))
                            {
                                subNode["name"] = name;
                            }
                        }
                    }
                }
            }
        }
    }

    public void UpdateFunctionVisibility(ref JToken source, SelectionResult selectionResult, string name)
    {
        foreach (var interestedFunctions in selectionResult.InterestedFunctions)
        {
            if (source["nodes"] is JArray nodes)
            {
                foreach (var child in nodes.Children())
                {
                    if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") &&
                        child["name"].Matches(interestedFunctions.Value))
                    {
                        var subNodes = child["nodes"].ToSafeList();

                        foreach (var subNode in subNodes)
                        {
                            if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function") &&
                                subNode["name"].Matches(interestedFunctions.Key))
                            {
                                subNode["visibility"] = name;
                            }
                        }
                    }
                }
            }
        }
    }
}