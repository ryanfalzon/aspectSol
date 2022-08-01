using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Modifications;

public class AddModificationNode : ModificationNode
{
    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(AddModificationNode)}>");
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(AddModificationNode)}>");

        return stringBuilder.ToString();
    }

    public override JToken Evaluate(JToken contract, FilteringResult filteringResult, JToken content, AbstractFilteringService abstractFilteringService)
    {
        abstractFilteringService.SourceManipulation.AddDefinitionToFunction(ref contract, content, filteringResult);
        return contract;
    }
}