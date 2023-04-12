using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Locations;

public abstract class LocationNode : Node
{
    public abstract FilteringResult[] Evaluate(JToken contract, AbstractFilteringService abstractFilteringService, IEnumerable<SelectorNode> selectors);
}