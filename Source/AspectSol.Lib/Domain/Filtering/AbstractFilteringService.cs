namespace AspectSol.Lib.Domain.Filtering;

public class AbstractFilteringService
{
    public IContractFiltering ContractFiltering { get; }
    public IFunctionFiltering FunctionFiltering { get; }
    public ISourceManipulation SourceManipulation { get; }
    public IVariableDefinitionFiltering VariableDefinitionFiltering { get; }
    public IVariableInteractionFiltering VariableGettersFiltering { get; }
    public IVariableInteractionFiltering VariableSettersFiltering { get; }

    public AbstractFilteringService(IContractFiltering contractFiltering, IFunctionFiltering functionFiltering, ISourceManipulation sourceManipulation,
        IVariableDefinitionFiltering variableDefinitionFiltering, IVariableInteractionFiltering variableGettersFiltering,
        IVariableInteractionFiltering variableSettersFiltering)
    {
        ContractFiltering           = contractFiltering;
        FunctionFiltering           = functionFiltering;
        SourceManipulation          = sourceManipulation;
        VariableDefinitionFiltering = variableDefinitionFiltering;
        VariableGettersFiltering    = variableGettersFiltering;
        VariableSettersFiltering    = variableSettersFiltering;
    }
}