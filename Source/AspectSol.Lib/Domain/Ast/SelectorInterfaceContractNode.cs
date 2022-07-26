using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SelectorInterfaceContractNode : SelectorNode
{
    public string ContractName { get; init; }

    public string InterfaceName { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorInterfaceContractNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<ContractName>{ContractName}</ContractName>");
        stringBuilder.AppendLine($"{GetIndentation()}<InterfaceName>{InterfaceName}</InterfaceName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorInterfaceContractNode)}>");

        return stringBuilder.ToString();
    }

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var contractNameSelectionResult = abstractFilteringService.ContractFiltering.FilterContractsByContractName(smartContract, ContractName);
        var interfaceNameSelectionResult = abstractFilteringService.ContractFiltering.FilterContractsByInterfaceName(smartContract, InterfaceName);
        
        return new SelectionResult
        {
            InterestedContracts   = contractNameSelectionResult.InterestedContracts.SafetIntersect(interfaceNameSelectionResult.InterestedContracts),
            InterestedFunctions   = contractNameSelectionResult.InterestedFunctions.SafetIntersect(interfaceNameSelectionResult.InterestedFunctions),
            InterestedDefinitions = contractNameSelectionResult.InterestedDefinitions.SafetIntersect(interfaceNameSelectionResult.InterestedDefinitions),
            InterestedStatements  = contractNameSelectionResult.InterestedStatements.SafetIntersect(interfaceNameSelectionResult.InterestedStatements),
        };
    }
}