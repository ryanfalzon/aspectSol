using System.Text;
using AspectSol.Lib.Domain.Filtering;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.AST;

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

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var variableTypeSelectionResult = VariableType.Filter(smartContract, abstractFilteringService);
        var variableLocationSelectionResult = VariableLocation.Filter(smartContract, abstractFilteringService);
        var variableNameSelectorSelectionResult = VariableNameSelector.Filter(smartContract, abstractFilteringService);
        var variableDecoratorSelectionResult = DecoratorVariable.Filter(smartContract, abstractFilteringService);

        var selectionResult = new SelectionResult
        {
            InterestedContracts = variableTypeSelectionResult.InterestedContracts.Intersect(variableLocationSelectionResult.InterestedContracts).Intersect
                (variableNameSelectorSelectionResult.InterestedContracts).Intersect(variableDecoratorSelectionResult.InterestedContracts).ToList(),
            InterestedFunctions = variableTypeSelectionResult.InterestedFunctions.Intersect(variableLocationSelectionResult.InterestedFunctions).Intersect
                (variableNameSelectorSelectionResult.InterestedFunctions).Intersect(variableDecoratorSelectionResult.InterestedFunctions).ToDictionary(item
                => item.Key, item => item.Value),
            InterestedDefinitions = variableTypeSelectionResult.InterestedDefinitions.Intersect(variableLocationSelectionResult.InterestedDefinitions).Intersect
                (variableNameSelectorSelectionResult.InterestedDefinitions).Intersect(variableDecoratorSelectionResult.InterestedDefinitions).ToDictionary(item
                => item.Key, item => item.Value),
            InterestedStatements = variableTypeSelectionResult.InterestedStatements.Intersect(variableLocationSelectionResult.InterestedStatements).Intersect
                (variableNameSelectorSelectionResult.InterestedStatements).Intersect(variableDecoratorSelectionResult.InterestedStatements).ToDictionary(item
                => item.Key, item => item.Value),
        };
        return selectionResult;
    }
}