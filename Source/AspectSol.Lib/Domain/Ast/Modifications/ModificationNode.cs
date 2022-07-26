﻿using AspectSol.Lib.Domain.Filtering;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Modifications;

public abstract class ModificationNode : Node
{
    public abstract JToken Evaluate(JToken contract, SelectionResult selectionResult, JToken content, AbstractFilteringService abstractFilteringService);
}