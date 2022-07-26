using System.Text;
using AspectSol.Lib.Domain.Filtering;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Placements;

public class BeforePlacementNode : PlacementNode
{
    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(BeforePlacementNode)}>");
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(BeforePlacementNode)}>");

        return stringBuilder.ToString();
    }

    public override JToken Evaluate(JToken contract, SelectionResult selectionResult, JToken content, AbstractFilteringService abstractFilteringService)
    {
        if (selectionResult.InterestedFunctions != null && selectionResult.InterestedFunctions.Count != 0)
        {
            abstractFilteringService.SourceManipulation.AddDefinitionToFunction(ref contract, content, selectionResult);
            return contract;
        }

        return contract;
    }
}