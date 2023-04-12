using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class DecoratorDefinitionImplementingNode : DecoratorDefinitionNode
{
    public string InterfaceName { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(DecoratorDefinitionImplementingNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(InterfaceName);

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(DecoratorDefinitionImplementingNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService, Location location)
    {
        var filteringResult = abstractFilteringService.ContractFiltering.FilterContractsByInterfaceName(smartContract, InterfaceName);
        return filteringResult;
    }
}