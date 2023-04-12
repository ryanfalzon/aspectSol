using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class DecoratorDefinitionTaggedNode : DecoratorDefinitionNode
{
    public SyntaxModifierNode SyntaxModifier { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(DecoratorDefinitionTaggedNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(SyntaxModifier.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(DecoratorDefinitionTaggedNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService, Location location)
    {
        var filteringResult = SyntaxModifier.Filter(smartContract, abstractFilteringService, location);
        return filteringResult;
    }
}