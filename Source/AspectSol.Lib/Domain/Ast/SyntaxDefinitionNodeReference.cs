using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SyntaxDefinitionNodeReference : SyntaxDefinitionNode
{
    public SelectorNode ContractSelector { get; init; }

    public SelectorNode FunctionSelector { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SyntaxDefinitionNodeReference)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(ContractSelector.ToString());
        stringBuilder.AppendLine(FunctionSelector.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SyntaxDefinitionNodeReference)}>");

        return stringBuilder.ToString();
    }

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var contractSelectorSelectionResult = ContractSelector.Filter(smartContract, abstractFilteringService);
        var functionSelectorSelectionResult = FunctionSelector?.Filter(smartContract, abstractFilteringService);
        
        return new SelectionResult
        {
            InterestedContracts   = contractSelectorSelectionResult.InterestedContracts.SafetIntersect(functionSelectorSelectionResult?.InterestedContracts),
            InterestedFunctions   = contractSelectorSelectionResult.InterestedFunctions.SafetIntersect(functionSelectorSelectionResult?.InterestedFunctions),
            InterestedDefinitions = contractSelectorSelectionResult.InterestedDefinitions.SafetIntersect(functionSelectorSelectionResult?.InterestedDefinitions),
            InterestedStatements  = contractSelectorSelectionResult.InterestedStatements.SafetIntersect(functionSelectorSelectionResult?.InterestedStatements)
        };
    }
}