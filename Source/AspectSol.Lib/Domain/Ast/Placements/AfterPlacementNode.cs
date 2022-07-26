using System.Text;
using AspectSol.Lib.Domain.Filtering;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Placements;

public class AfterPlacementNode : PlacementNode
{
    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(AfterPlacementNode)}>");
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(AfterPlacementNode)}>");

        return stringBuilder.ToString();
    }

    public override JToken Evaluate(JToken contract, SelectionResult selectionResult, JToken content, AbstractFilteringService abstractFilteringService)
    {
        if (selectionResult.InterestedFunctions != null && selectionResult.InterestedFunctions.Count != 0)
        {
            abstractFilteringService.SourceManipulation.AddDefinitionToFunction(ref contract, content, selectionResult, false);
            return contract;
        }

        return contract;
    }
}