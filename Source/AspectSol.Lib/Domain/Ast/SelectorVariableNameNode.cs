﻿using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SelectorVariableNameNode : SelectorNode
{
    public string VariableName { get; init; }
    
    private VariableAccess VariableAccess { get; }
    
    public SelectorVariableNameNode(VariableAccess variableAccess)
    {
        VariableAccess = variableAccess;
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorVariableNameNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<VariableName>{VariableName}</VariableName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorVariableNameNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var filteringResult = VariableAccess switch
        {
            VariableAccess.Get => abstractFilteringService.VariableGettersFiltering.FilterVariableInteractionByVariableName(smartContract, VariableName),
            VariableAccess.Set => abstractFilteringService.VariableSettersFiltering.FilterVariableInteractionByVariableName(smartContract, VariableName),
            _                  => throw new ArgumentOutOfRangeException()
        };
        return filteringResult;
    }
}