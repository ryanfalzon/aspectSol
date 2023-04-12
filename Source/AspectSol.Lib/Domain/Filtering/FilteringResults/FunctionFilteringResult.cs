namespace AspectSol.Lib.Domain.Filtering.FilteringResults;

public class FunctionFilteringResult
{
    public string FunctionName { get; }
    public List<StatementFilteringResult> StatementFilteringResults { get; private set; }

    public FunctionFilteringResult(string functionName, List<StatementFilteringResult> statementFilteringResults = null)
    {
        FunctionName              = functionName;
        StatementFilteringResults = statementFilteringResults ?? new List<StatementFilteringResult>();
    }
    
    public void ReplaceStatementFilteringResults(List<StatementFilteringResult> statementFilteringResults)
    {
        StatementFilteringResults = statementFilteringResults;
    }
}