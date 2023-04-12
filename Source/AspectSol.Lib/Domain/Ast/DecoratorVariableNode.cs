using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class DecoratorVariableNode : DecoratorNode
{
    public VariableVisibility VariableVisibility { get; init; }
    
    private VariableAccess VariableAccess { get; }
    
    public DecoratorVariableNode(VariableAccess variableAccess)
    {
        VariableAccess = variableAccess;
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(DecoratorVariableNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<VariableVisibility>{VariableVisibility}</VariableVisibility>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(DecoratorVariableNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService, Location location)
    {
        var selectionResult = VariableAccess switch
        {
            VariableAccess.Get => abstractFilteringService.VariableGettersFiltering.FilterVariableInteractionByVariableVisibility(smartContract,
                VariableVisibility.GetDescription()),
            VariableAccess.Set => abstractFilteringService.VariableSettersFiltering.FilterVariableInteractionByVariableVisibility(smartContract,
                VariableVisibility.GetDescription()),
            _ => throw new ArgumentOutOfRangeException()
        };
        return selectionResult;
    }
}