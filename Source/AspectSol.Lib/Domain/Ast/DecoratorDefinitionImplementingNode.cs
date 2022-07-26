using System.Text;
using AspectSol.Lib.Domain.Filtering;
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

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var selectionResult = abstractFilteringService.ContractFiltering.FilterContractsByInterfaceName(smartContract, InterfaceName);
        return selectionResult;
    }
}