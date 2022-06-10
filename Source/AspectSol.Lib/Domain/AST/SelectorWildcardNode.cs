using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Infra.Enums;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.AST;

public class SelectorWildcardNode : SelectorNode
{
    private const string Token = "*";

    public WildcardFor WildcardFor { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorWildcardNode)}></{nameof(SelectorWildcardNode)}>");

        return stringBuilder.ToString();
    }

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var selectionResult = WildcardFor switch
        {
            WildcardFor.Contract           => abstractFilteringService.ContractFiltering.FilterContractsByContractName(smartContract, Token),
            WildcardFor.Function           => abstractFilteringService.FunctionFiltering.FilterFunctionsByFunctionName(smartContract, Token),
            WildcardFor.VariableNameGetter => abstractFilteringService.VariableGettersFiltering.FilterVariableInteractionByVariableName(smartContract, Token),
            WildcardFor.VariableTypeGetter => abstractFilteringService.VariableGettersFiltering.FilterVariableInteractionByVariableType(smartContract, Token),
            WildcardFor.VariableNameSetter => abstractFilteringService.VariableSettersFiltering.FilterVariableInteractionByVariableName(smartContract, Token),
            WildcardFor.VariableTypeSetter => abstractFilteringService.VariableSettersFiltering.FilterVariableInteractionByVariableType(smartContract, Token),
            _                              => throw new ArgumentOutOfRangeException()
        };

        return selectionResult;
    }
}