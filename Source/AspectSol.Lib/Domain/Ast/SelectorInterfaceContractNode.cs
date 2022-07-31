using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Helpers.FilteringResults;
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

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var contractNameFilteringResult = abstractFilteringService.ContractFiltering.FilterContractsByContractName(smartContract, ContractName);
        var interfaceNameFilteringResult = abstractFilteringService.ContractFiltering.FilterContractsByInterfaceName(smartContract, InterfaceName);

        var intersectionResult = FilteringResultHelpers.Intersect(contractNameFilteringResult, interfaceNameFilteringResult);
        return intersectionResult;
    }
}