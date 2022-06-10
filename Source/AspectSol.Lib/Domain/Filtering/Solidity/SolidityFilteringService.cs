namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class SolidityFilteringService : AbstractFilteringService
{
    public SolidityFilteringService(ContractFiltering contractFiltering, FunctionFiltering functionFiltering, SourceManipulation sourceManipulation,
        VariableDefinitionFiltering variableDefinitionFiltering, VariableGettersFiltering variableGettersFiltering,
        VariableSettersFiltering variableSettersFiltering) : base(contractFiltering, functionFiltering, sourceManipulation, variableDefinitionFiltering,
        variableGettersFiltering, variableSettersFiltering)
    {
    }
}