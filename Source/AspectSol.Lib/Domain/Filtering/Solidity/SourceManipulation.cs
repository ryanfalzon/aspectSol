using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json;
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

    public void AddDefinitionToContract(ref JToken contract, JToken definition, SelectionResult selectionResult, bool addFirst = true)
    {
        foreach (var interestedContract in selectionResult.InterestedContracts)
        {
            if (contract["nodes"] is JArray nodes)
            {
                foreach (var child in nodes.Children())
                {
                    if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") && child["name"].Matches(interestedContract))
                    {
                        var baseContracts = child["nodes"] as JArray ?? new JArray();

                        var newChildNodes = new JArray();
                        if (addFirst)
                        {
                            foreach (var newSubNode in GetContractSubNodes(definition))
                            {
                                newChildNodes.Add(newSubNode);
                            }

                            foreach (var baseContract in baseContracts)
                            {
                                newChildNodes.Add(baseContract);
                            }
                        }
                        else
                        {
                            foreach (var baseContract in baseContracts)
                            {
                                newChildNodes.Add(baseContract);
                            }
                            
                            foreach (var newSubNode in GetContractSubNodes(definition))
                            {
                                newChildNodes.Add(newSubNode);
                            }
                        }
                        
                        child["nodes"]?.Replace(newChildNodes);
                    }
                }
            }
        }
    }

    public void AddDefinitionsToContract(ref JToken contract, List<JToken> definitions, SelectionResult selectionResult, bool addFirst = true)
    {
        foreach (var definition in definitions)
        {
            AddDefinitionToContract(ref contract, definition, selectionResult, addFirst);
        }
    }

    public void AddDefinitionToFunction(ref JToken contract, JToken definition, SelectionResult selectionResult, bool addFirst = true)
    {
        foreach (var interestedFunctions in selectionResult.InterestedFunctions)
        {
            if (contract["nodes"] is JArray nodes)
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
                                var statements = subNode["body"]?["statements"] as JArray ?? new JArray();
                                
                                var newChildNodes = new JArray();
                                if (addFirst)
                                {
                                    foreach (var newSubNode in GetFunctionSubNodes(definition))
                                    {
                                        newChildNodes.Add(newSubNode);
                                    }

                                    foreach (var statement in statements)
                                    {
                                        newChildNodes.Add(statement);
                                    }
                                }
                                else
                                {
                                    foreach (var statement in statements)
                                    {
                                        newChildNodes.Add(statement);
                                    }
                            
                                    foreach (var newSubNode in GetFunctionSubNodes(definition))
                                    {
                                        newChildNodes.Add(newSubNode);
                                    }
                                }
                        
                                subNode["body"]?["statements"]?.Replace(newChildNodes);
                            }
                        }
                    }
                }
            }
        }
    }

    public void AddDefinitionsToFunction(ref JToken source, List<JToken> definitions, SelectionResult selectionResult, bool addFirst = true)
    {
        foreach (var definition in definitions)
        {
            AddDefinitionToFunction(ref source, definition, selectionResult, addFirst);
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
    
    private List<JToken> GetContractSubNodes(JToken contract)
    {
        if (contract["nodes"] is JArray nodes)
        {
            foreach (var child in nodes.Children())
            {
                if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract"))
                {
                    return child["nodes"].ToSafeList();
                }
            }
        }

        return new List<JToken>();
    }

    private List<JToken> GetFunctionSubNodes(JToken contract)
    {
        if (contract["nodes"] is JArray nodes)
        {
            foreach (var child in nodes.Children())
            {
                if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract"))
                {
                    var subNodes = child["nodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function"))
                        {
                            return subNode["body"]?["statements"].ToSafeList();
                        }
                    }
                }
            }
        }
        
        return new List<JToken>();
    }
}