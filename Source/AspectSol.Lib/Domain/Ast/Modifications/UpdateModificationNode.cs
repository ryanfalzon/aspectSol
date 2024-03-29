﻿using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Modifications;

public class UpdateModificationNode : ModificationNode
{
    public override string ToString()
    {
        StringBuilder stringBuilder = new();
        
        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(UpdateModificationNode)}>");
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(UpdateModificationNode)}>");

        return stringBuilder.ToString();
    }

    public override JToken Evaluate(JToken contract, FilteringResult filteringResult, JToken content, AbstractFilteringService abstractFilteringService)
    {
        throw new NotImplementedException();
    }
}