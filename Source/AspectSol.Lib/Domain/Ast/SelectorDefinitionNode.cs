using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Infra.Helpers;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SelectorDefinitionNode : SelectorNode
{
    public SyntaxDefinitionNode SyntaxDefinition { get; set; }

    public DecoratorDefinitionNode DecoratorDefinition { get; set; }

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

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var definitionSyntaxSelectionResult = SyntaxDefinition?.Filter(smartContract, abstractFilteringService);
        var definitionDecoratorSelectionResult = DecoratorDefinition?.Filter(smartContract, abstractFilteringService);

        var selectionResult = SelectionResultHelpers.Intersect(definitionSyntaxSelectionResult, definitionDecoratorSelectionResult);
        return selectionResult;
    }
}