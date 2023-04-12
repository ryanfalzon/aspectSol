using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Selectors;

public class WildcardSelectorNode : SelectorNode
{
    private const string Token = "*";

    public WildcardFor WildcardFor { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(WildcardSelectorNode)}></{nameof(WildcardSelectorNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService, Location location)
    {
        var selectionResult = WildcardFor switch
        {
            WildcardFor.Contract           => abstractFilteringService.ContractFiltering.FilterContractsByContractName(smartContract, Token),
            WildcardFor.Function           => abstractFilteringService.FunctionFiltering.FilterFunctionsByFunctionName(smartContract, Token),
            WildcardFor.VariableNameGetter => abstractFilteringService.VariableGettersFiltering.FilterVariableInteractionByVariableName(smartContract, Token),
            WildcardFor.VariableTypeGetter => abstractFilteringService.VariableGettersFiltering.FilterVariableInteractionByVariableType(smartContract, Token),
            WildcardFor.VariableNameSetter => abstractFilteringService.VariableSettersFiltering.FilterVariableInteractionByVariableName(smartContract, Token),
            WildcardFor.VariableTypeSetter => abstractFilteringService.VariableSettersFiltering.FilterVariableInteractionByVariableType(smartContract, Token),
            WildcardFor.DictionaryElement  => throw new NotSupportedException("Wildcard for dictionary element is not supported yet"),
            _                              => throw new ArgumentOutOfRangeException($"Wildcard for [{WildcardFor}] out of supported range")
        };

        return selectionResult;
    }
}