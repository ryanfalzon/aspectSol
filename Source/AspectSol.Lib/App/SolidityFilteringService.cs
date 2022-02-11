using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.Solidity;

namespace AspectSol.Lib.App;

public class SolidityFilteringService
{
    public IContractFiltering ContractFiltering { get; set; }
    public IFunctionFiltering FunctionFiltering { get; set; }
    public INodeGenerator NodeGenerator { get; set; }
    public ISourceManipulation SourceManipulation { get; set; }
    public IVariableDefinitionFiltering VariableDefinitionFiltering { get; set; }
    public VariableGettersFiltering VariableGettersFiltering { get; set; }
    public VariableSettersFiltering VariableSettersFiltering { get; set; }

    public SolidityFilteringService(IContractFiltering contractFiltering, IFunctionFiltering functionFiltering, INodeGenerator nodeGenerator,
        ISourceManipulation sourceManipulation, IVariableDefinitionFiltering variableDefinitionFiltering, VariableGettersFiltering variableGettersFiltering,
        VariableSettersFiltering variableSettersFiltering)
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