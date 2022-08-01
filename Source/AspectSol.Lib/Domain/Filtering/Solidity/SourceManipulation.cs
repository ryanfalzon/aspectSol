using AspectSol.Lib.Domain.Filtering.FilteringResults;
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

    public void AddInterfaceToContract(ref JToken source, JToken contractInterface, FilteringResult filteringResult)
    {
        foreach (var contract in filteringResult.ContractFilteringResults)
        {
            if (source["nodes"] is JArray nodes)
            {
                foreach (var child in nodes.Children())
                {
                    if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") && 
                        child["name"].Matches(contract.ContractName))
                    {
                        var baseContracts = child["baseContracts"] as JArray;
                        baseContracts?.AddFirst(contractInterface);
                    }
                }
            }
        }
    }

    public void AddDefinitionToContract(ref JToken source, JToken definition, FilteringResult filteringResult, bool addFirst = true)
    {
        foreach (var contract in filteringResult.ContractFilteringResults)
        {
            if (source["nodes"] is JArray nodes)
            {
                foreach (var child in nodes.Children())
                {
                    if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") && 
                        child["name"].Matches(contract.ContractName))
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

    public void AddDefinitionsToContract(ref JToken source, List<JToken> definitions, FilteringResult filteringResult, bool addFirst = true)
    {
        foreach (var definition in definitions)
        {
            AddDefinitionToContract(ref source, definition, filteringResult, addFirst);
        }
    }

    public void AddDefinitionToFunction(ref JToken source, JToken definition, FilteringResult filteringResult, bool addFirst = true)
    {
        foreach (var contract in filteringResult.ContractFilteringResults)
        {
            foreach (var function in contract.FunctionFilteringResults)
            {
                if (source["nodes"] is JArray nodes)
                {
                    foreach (var child in nodes.Children())
                    {
                        if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") &&
                            child["name"].Matches(contract.ContractName))
                        {
                            var subNodes = child["nodes"].ToSafeList();

                            foreach (var subNode in subNodes)
                            {
                                if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function") &&
                                    subNode["name"].Matches(function.FunctionName))
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
    }

    public void AddDefinitionsToFunction(ref JToken source, List<JToken> definitions, FilteringResult filteringResult, bool addFirst = true)
    {
        foreach (var definition in definitions)
        {
            AddDefinitionToFunction(ref source, definition, filteringResult, addFirst);
        }
    }

    public void UpdateContractName(ref JToken source, FilteringResult filteringResult, string name)
    {
        foreach (var contract in filteringResult.ContractFilteringResults)
        {
            if (source["nodes"] is JArray nodes)
            {
                foreach (var child in nodes.Children())
                {
                    if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") && 
                        child["name"].Matches(contract.ContractName))
                    {
                        child["name"] = name;
                    }
                }
            }
        }
    }

    public void UpdateFunctionName(ref JToken source, FilteringResult filteringResult, string name)
    {
        foreach (var contract in filteringResult.ContractFilteringResults)
        {
            foreach (var function in contract.FunctionFilteringResults)
            {
                if (source["nodes"] is JArray nodes)
                {
                    foreach (var child in nodes.Children())
                    {
                        if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") &&
                            child["name"].Matches(contract.ContractName))
                        {
                            var subNodes = child["nodes"].ToSafeList();

                            foreach (var subNode in subNodes)
                            {
                                if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function") &&
                                    subNode["name"].Matches(function.FunctionName))
                                {
                                    subNode["name"] = name;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void UpdateFunctionVisibility(ref JToken source, FilteringResult filteringResult, string name)
    {
        foreach (var contract in filteringResult.ContractFilteringResults)
        {
            foreach (var function in contract.FunctionFilteringResults)
            {
                if (source["nodes"] is JArray nodes)
                {
                    foreach (var child in nodes.Children())
                    {
                        if (child["nodeType"].Matches(ContractDefinition) && child["contractKind"].Matches("contract") &&
                            child["name"].Matches(contract.ContractName))
                        {
                            var subNodes = child["nodes"].ToSafeList();

                            foreach (var subNode in subNodes)
                            {
                                if (subNode["nodeType"].Matches(FunctionDefinition) && subNode["kind"].Matches("function") &&
                                    subNode["name"].Matches(function.FunctionName))
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