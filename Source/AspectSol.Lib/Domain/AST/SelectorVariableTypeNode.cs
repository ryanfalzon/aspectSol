using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Infra.Enums;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.AST;

public class SelectorVariableTypeNode : SelectorNode
{
    public string VariableType { get; }
    
    private VariableAccess VariableAccess { get; }
    
    public SelectorVariableTypeNode(VariableAccess variableAccess, string variableType)
    {
        VariableAccess = variableAccess;
        VariableType   = variableType;
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorVariableTypeNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<VariableType>{VariableType}</VariableType>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorVariableTypeNode)}>");

        return stringBuilder.ToString();
    }

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var selectionResult = VariableAccess switch
        {
            VariableAccess.Get => abstractFilteringService.VariableGettersFiltering.FilterVariableInteractionByVariableType(smartContract, VariableType),
            VariableAccess.Set => abstractFilteringService.VariableSettersFiltering.FilterVariableInteractionByVariableType(smartContract, VariableType),
            _                  => throw new ArgumentOutOfRangeException()
        };
        return selectionResult;
    }
}