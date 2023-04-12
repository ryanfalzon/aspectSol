using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Locations;

public class CallToNode : LocationNode
{
    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(CallToNode)}>");
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(CallToNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult[] Evaluate(JToken contract, AbstractFilteringService abstractFilteringService, IEnumerable<SelectorNode> selectors)
    {
        var filteringResults = selectors.Select(x => x.Filter(contract, abstractFilteringService, Location.CallTo)).ToArray();
        return filteringResults;
    }
}