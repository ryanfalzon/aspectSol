using System.Reflection.Metadata;
using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using Microsoft.VisualBasic;
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

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService, Location location)
    {
        var filteringResult = location switch
        {
            Location.CallTo      => abstractFilteringService.FunctionFiltering.FilterFunctionCallsByInstanceName(smartContract, "*", FunctionName),
            Location.ExecutionOf => abstractFilteringService.FunctionFiltering.FilterFunctionsByFunctionName(smartContract, FunctionName),
            _                    => throw new NotSupportedException($"Location '{location}' is not supported")
        };
        return filteringResult;
    }
}