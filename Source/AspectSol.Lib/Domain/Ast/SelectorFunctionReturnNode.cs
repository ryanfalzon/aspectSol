using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SelectorFunctionReturnNode : SelectorNode
{
    public List<string> Returns { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorFunctionReturnNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(string.Join('n', Returns));

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorFunctionReturnNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var filteringResult = abstractFilteringService.FunctionFiltering.FilterFunctionsByReturnParameters(smartContract, Returns);
        return filteringResult;
    }
}