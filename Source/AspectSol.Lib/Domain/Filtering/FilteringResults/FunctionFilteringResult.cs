namespace AspectSol.Lib.Domain.Filtering.FilteringResults;

public class FunctionFilteringResult
{
    public string FunctionName { get; }
    public List<StatementFilteringResult> StatementFilteringResults { get; private set; }

    public FunctionFilteringResult(string functionName)
    {
        FunctionName              = functionName;
        StatementFilteringResults = new List<StatementFilteringResult>();
    }
    
    public void ReplaceStatementFilteringResults(List<StatementFilteringResult> statementFilteringResults)
    {
        StatementFilteringResults = statementFilteringResults;
    }
}