using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
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

    public override JToken Evaluate(JToken contract, FilteringResult filteringResult, JToken content, AbstractFilteringService abstractFilteringService)
    {
        abstractFilteringService.SourceManipulation.AddDefinitionToFunction(ref contract, content, filteringResult);
        return contract;
    }
}