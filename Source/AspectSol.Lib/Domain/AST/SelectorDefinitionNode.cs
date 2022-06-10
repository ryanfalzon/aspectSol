using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.AST;

public class SelectorDefinitionNode : SelectorNode
{
    public SyntaxDefinitionNode SyntaxDefinition { get; set; }

    public List<ParameterNode> Parameters { get; set; }

    public DecoratorDefinitionNode DecoratorDefinition { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorDefinitionNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(SyntaxDefinition.ToString());
        stringBuilder.AppendLine(string.Join('\n', Parameters));
        stringBuilder.AppendLine(DecoratorDefinition.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorDefinitionNode)}>");

        return stringBuilder.ToString();
    }

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var definitionSyntaxSelectionResult = SyntaxDefinition.Filter(smartContract, abstractFilteringService);

        var parameters = Parameters != null 
            ? Parameters.Select(item => (item.Type, item.Name)).ToList() 
            : new List<(string Type, string Value)>();
        var parametersSelectionResult = abstractFilteringService.FunctionFiltering.FilterFunctionsByParameters(smartContract, parameters);

        var definitionDecoratorSelectionResult = DecoratorDefinition.Filter(smartContract, abstractFilteringService);

        return new SelectionResult
        {
            InterestedContracts = definitionSyntaxSelectionResult.InterestedContracts.SafetIntersect(parametersSelectionResult.InterestedContracts)
                .SafetIntersect(definitionDecoratorSelectionResult.InterestedContracts),
            InterestedFunctions = definitionSyntaxSelectionResult.InterestedFunctions.SafetIntersect(parametersSelectionResult.InterestedFunctions)
                .SafetIntersect(definitionDecoratorSelectionResult.InterestedFunctions),
            InterestedDefinitions = definitionSyntaxSelectionResult.InterestedDefinitions.SafetIntersect(parametersSelectionResult.InterestedDefinitions)
                .SafetIntersect(definitionDecoratorSelectionResult.InterestedDefinitions),
            InterestedStatements = definitionSyntaxSelectionResult.InterestedStatements.SafetIntersect(parametersSelectionResult.InterestedStatements)
                .SafetIntersect(definitionDecoratorSelectionResult.InterestedStatements)
        };
    }
}