using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Selectors.Function;

public class FunctionNameSelectorNode : SelectorNode
{
    public string FunctionName { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(FunctionNameSelectorNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<FunctionName>{FunctionName}</FunctionName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(FunctionNameSelectorNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var selectionResult = abstractFilteringService.FunctionFiltering.FilterFunctionsByFunctionName(smartContract, FunctionName);
        return selectionResult;
    }
}