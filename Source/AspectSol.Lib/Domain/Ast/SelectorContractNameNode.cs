using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SelectorContractNameNode : SelectorNode
{
    public string ContractName { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorContractNameNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<ContractName>{ContractName}</ContractName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorContractNameNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService, Location location)
    {
        var filteringResult = abstractFilteringService.ContractFiltering.FilterContractsByContractName(smartContract, ContractName);
        return filteringResult;
    }
}