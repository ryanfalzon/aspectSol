namespace AspectSol.Lib.Domain.Filtering.FilteringResults;

public class DefinitionFilteringResult
{
    public string DefinitionName { get; }

    public DefinitionFilteringResult(string definitionName)
    {
        DefinitionName = definitionName;
    }
}