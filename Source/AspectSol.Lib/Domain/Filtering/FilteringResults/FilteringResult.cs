namespace AspectSol.Lib.Domain.Filtering.FilteringResults;

public class FilteringResult
{
    public List<ContractFilteringResult> ContractFilteringResults { get; private set; }

    public FilteringResult()
    {
        ContractFilteringResults = new List<ContractFilteringResult>();
    }

    public void AddContract(string contractName)
    {
        ContractFilteringResults.Add(new ContractFilteringResult(contractName));
    }
    
    public void ReplaceContractFilteringResults(List<ContractFilteringResult> contractFilteringResults)
    {
        ContractFilteringResults = contractFilteringResults;
    }

    public void AddDefinition(string contractName, string definitionName)
    {
        if (!ContractFilteringResults.Exists(x => x.ContractName == contractName)) AddContract(contractName);

        ContractFilteringResults
            .Single(x => x.ContractName == contractName).DefinitionFilteringResults
            .Add(new DefinitionFilteringResult(definitionName));
    }

    public void AddFunction(string contractName, string functionName)
    {
        if (!ContractFilteringResults.Exists(x => x.ContractName == contractName)) AddContract(contractName);

        ContractFilteringResults
            .Single(x => x.ContractName == contractName).FunctionFilteringResults
            .Add(new FunctionFilteringResult(functionName));
    }

    public void AddStatement(string contractName, string functionName, int statementIndex)
    {
        if (!ContractFilteringResults.Exists(x => x.ContractName == contractName)) AddContract(contractName);

        var contract = ContractFilteringResults.Single(x => x.ContractName == contractName);

        if (!contract.HasFunction(functionName)) AddFunction(contractName, functionName);

        ContractFilteringResults
            .Single(x => x.ContractName == contractName).FunctionFilteringResults
            .Single(x => x.FunctionName == functionName).StatementFilteringResults
            .Add(new StatementFilteringResult(statementIndex));
    }

    public void AddStatement(string contractName, string functionName, IEnumerable<int> statementIndices)
    {
        if (!ContractFilteringResults.Exists(x => x.ContractName == contractName)) AddContract(contractName);

        var contract = ContractFilteringResults.Single(x => x.ContractName == contractName);

        if (!contract.HasFunction(functionName)) AddFunction(contractName, functionName);

        ContractFilteringResults
            .Single(x => x.ContractName == contractName).FunctionFilteringResults
            .Single(x => x.FunctionName == functionName).StatementFilteringResults
            .AddRange(statementIndices.Select(x => new StatementFilteringResult(x)));
    }

    public void AddStatement(string contractName, string functionName, List<StatementFilteringResult> statementFilteringResults)
    {
        if (!ContractFilteringResults.Exists(x => x.ContractName == contractName)) AddContract(contractName);

        var contract = ContractFilteringResults.Single(x => x.ContractName == contractName);

        if (!contract.HasFunction(functionName)) AddFunction(contractName, functionName);

        ContractFilteringResults
            .Single(x => x.ContractName == contractName).FunctionFilteringResults
            .Single(x => x.FunctionName == functionName).StatementFilteringResults
            .AddRange(statementFilteringResults);
    }

    public void AddStatement(string contractName, string functionName, params int[] nestedStatementIndices)
    {
        if (!ContractFilteringResults.Exists(x => x.ContractName == contractName)) AddContract(contractName);

        var contract = ContractFilteringResults.Single(x => x.ContractName == contractName);

        if (!contract.HasFunction(functionName)) AddFunction(contractName, functionName);

        var statement = new StatementFilteringResult(nestedStatementIndices[0]);
        for (var i = 1; i < nestedStatementIndices.Length; i++)
        {
            var nextStatement = new StatementFilteringResult(nestedStatementIndices[i], new List<StatementFilteringResult> {statement});
            statement = nextStatement;
        }

        ContractFilteringResults
            .Single(x => x.ContractName == contractName).FunctionFilteringResults
            .Single(x => x.FunctionName == functionName).StatementFilteringResults
            .Add(statement);
    }
}