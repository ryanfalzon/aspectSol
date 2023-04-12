using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Locations;

public class ExecutionOfNode : LocationNode
{
    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(ExecutionOfNode)}>");
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(ExecutionOfNode)}>");

        return stringBuilder.ToString();
    }
    
    public override FilteringResult[] Evaluate(JToken contract, AbstractFilteringService abstractFilteringService, IEnumerable<SelectorNode> selectors)
    {
        var filteringResults = selectors.Select(x => x.Filter(contract, abstractFilteringService, Location.ExecutionOf)).ToArray();
        return filteringResults;
    }
}