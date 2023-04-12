using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using AspectSol.Lib.Infra.Helpers.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SelectorVariableNode : SelectorNode
{
    public VariableAccessNode VariableAccessNode { get; set; }

    public SelectorNode VariableType { get; set; }

    public SelectorNode VariableLocation { get; set; }

    public SelectorNode VariableNameSelector { get; set; }

    public DecoratorVariableNode DecoratorVariable { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorVariableNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(VariableAccessNode.ToString());
        stringBuilder.AppendLine(VariableType.ToString());
        stringBuilder.AppendLine(VariableLocation.ToString());
        stringBuilder.AppendLine(VariableNameSelector.ToString());
        stringBuilder.AppendLine(DecoratorVariable.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorVariableNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService, Location location)
    {
        var variableTypeFilteringResult = VariableType.Filter(smartContract, abstractFilteringService, location);
        var variableLocationFilteringResult = VariableLocation.Filter(smartContract, abstractFilteringService, location);
        var variableNameSelectorFilteringResult = VariableNameSelector.Filter(smartContract, abstractFilteringService, location);
        var variableDecoratorFilteringResult = DecoratorVariable.Filter(smartContract, abstractFilteringService, location);

        var intersectionResult = FilteringResultHelpers.Intersect(variableTypeFilteringResult, variableLocationFilteringResult,
            variableNameSelectorFilteringResult, variableDecoratorFilteringResult);
        return intersectionResult;
    }
}