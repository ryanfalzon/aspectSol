using System.Text;
using AspectSol.Lib.Domain.Filtering;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.AST;

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

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var selectionResult = SyntaxModifier.Filter(smartContract, abstractFilteringService);
        return selectionResult;
    }
}