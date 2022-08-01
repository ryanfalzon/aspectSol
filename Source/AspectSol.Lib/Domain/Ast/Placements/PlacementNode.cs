using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Placements;

public abstract class PlacementNode : Node
{
    public abstract JToken Evaluate(JToken contract, FilteringResult filteringResult, JToken content, AbstractFilteringService abstractFilteringService);
}