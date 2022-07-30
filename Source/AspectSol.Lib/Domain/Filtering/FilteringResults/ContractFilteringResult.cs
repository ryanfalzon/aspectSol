namespace AspectSol.Lib.Domain.Filtering.FilteringResults;

public class ContractFilteringResult
{
    public string ContractName { get; }
    public List<DefinitionFilteringResult> DefinitionFilteringResults { get; private set; }
    public List<FunctionFilteringResult> FunctionFilteringResults { get; private set; }

    public ContractFilteringResult(string contractName)
    {
        ContractName               = contractName;
        DefinitionFilteringResults = new List<DefinitionFilteringResult>();
        FunctionFilteringResults   = new List<FunctionFilteringResult>();
    }

    public bool HasFunction(string functionName)
    {
        return FunctionFilteringResults.Exists(x => x.FunctionName == functionName);
    }
    
    public void ReplaceDefinitionFilteringResults(List<DefinitionFilteringResult> definitionFilteringResults)
    {
        DefinitionFilteringResults = definitionFilteringResults;
    }
    
    public void ReplaceFunctionFilteringResults(List<FunctionFilteringResult> functionFilteringResults)
    {
        FunctionFilteringResults = functionFilteringResults;
    }
}