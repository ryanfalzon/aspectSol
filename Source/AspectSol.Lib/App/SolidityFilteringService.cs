using AspectSol.Lib.Domain.Filtering;

namespace AspectSol.Lib.App;

public class SolidityFilteringService
{
    public IContractFiltering ContractFiltering { get; set; }
    public IFunctionFiltering FunctionFiltering { get; set; }
    public INodeGenerator NodeGenerator { get; set; }
    public ISourceManipulation SourceManipulation { get; set; }
    public IVariableDefinitionFiltering VariableDefinitionFiltering { get; set; }
    public IVariableGettersFiltering VariableGettersFiltering { get; set; }
    public IVariableSettersFiltering VariableSettersFiltering { get; set; }

    public SolidityFilteringService(IContractFiltering contractFiltering, IFunctionFiltering functionFiltering, INodeGenerator nodeGenerator,
        ISourceManipulation sourceManipulation, IVariableDefinitionFiltering variableDefinitionFiltering, IVariableGettersFiltering variableGettersFiltering,
        IVariableSettersFiltering variableSettersFiltering)
    {
        ContractFiltering = contractFiltering;
        FunctionFiltering = functionFiltering;
        NodeGenerator = nodeGenerator;
        SourceManipulation = sourceManipulation;
        VariableDefinitionFiltering = variableDefinitionFiltering;
        VariableGettersFiltering = variableGettersFiltering;
        VariableSettersFiltering = variableSettersFiltering;
    }
}