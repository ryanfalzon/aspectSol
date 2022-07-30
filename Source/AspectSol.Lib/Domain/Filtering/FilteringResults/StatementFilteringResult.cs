namespace AspectSol.Lib.Domain.Filtering.FilteringResults;

public class StatementFilteringResult
{
    public int StatementIndex { get; }
    public List<StatementFilteringResult> StatementFilteringResults { get; private set; }

    public StatementFilteringResult(int statementIndex)
    {
        StatementIndex = statementIndex;
    }

    public StatementFilteringResult(int statementIndex, List<StatementFilteringResult> statementFilteringResults)
    {
        StatementIndex            = statementIndex;
        StatementFilteringResults = statementFilteringResults;
    }
    
    public void ReplaceStatementFilteringResults(List<StatementFilteringResult> statementFilteringResults)
    {
        StatementFilteringResults = statementFilteringResults;
    }
}