using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
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

    public override JToken Evaluate(JToken contract, FilteringResult filteringResult, JToken content, AbstractFilteringService abstractFilteringService)
    {
        abstractFilteringService.SourceManipulation.AddDefinitionToFunction(ref contract, content, filteringResult, false);
        return contract;
    }
}