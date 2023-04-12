using System.Text;
using AspectSol.Lib.Domain.Ast.Syntax;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using AspectSol.Lib.Infra.Helpers.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Selectors;

public class SelectorDefinitionNode : SelectorNode
{
    public SyntaxDefinitionNode SyntaxDefinition { get; init; }
    public DecoratorDefinitionNode DecoratorDefinition { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorDefinitionNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(SyntaxDefinition.ToString());
        stringBuilder.AppendLine(DecoratorDefinition.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorDefinitionNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService, Location location)
    {
        var definitionSyntaxFilteringResult = SyntaxDefinition?.Filter(smartContract, abstractFilteringService, location);
        var definitionDecoratorFilteringResult = DecoratorDefinition?.Filter(smartContract, abstractFilteringService, location);

        var filteringResult = FilteringResultHelpers.Intersect(definitionSyntaxFilteringResult, definitionDecoratorFilteringResult);
        return filteringResult;
    }
}