using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using AspectSol.Lib.Infra.Helpers.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Syntax;

public class ReferenceSyntaxDefinitionNode : SyntaxDefinitionNode
{
    public SelectorNode ContractSelector { get; init; }
    public SelectorNode FunctionSelector { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(ReferenceSyntaxDefinitionNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(ContractSelector.ToString());
        stringBuilder.AppendLine(FunctionSelector.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(ReferenceSyntaxDefinitionNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService, Location location)
    {
        var contractSelectorFilteringResult = ContractSelector.Filter(smartContract, abstractFilteringService, location);
        var functionSelectorFilteringResult = FunctionSelector?.Filter(smartContract, abstractFilteringService, location);

        var collationResult = functionSelectorFilteringResult == null
            ? contractSelectorFilteringResult
            : FunctionFilteringResultHelpers.Collate(functionSelectorFilteringResult, contractSelectorFilteringResult);
        
        return collationResult;
    }
}