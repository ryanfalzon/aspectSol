using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors.Function;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class FunctionNameSelectorFunctionParametersNode : FunctionNameSelectorNode
{
    public List<ParameterNode> Parameters { get; init; }

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
        var filteringResult = abstractFilteringService.FunctionFiltering.FilterFunctionsByFunctionName(smartContract, FunctionName);
        return filteringResult;
    }
}