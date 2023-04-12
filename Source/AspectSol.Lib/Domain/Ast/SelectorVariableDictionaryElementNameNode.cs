using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SelectorVariableDictionaryElementNameNode : SelectorVariableNameNode
{
    public SelectorNode KeySelector { get; set; }

    public SelectorVariableDictionaryElementNameNode(VariableAccess variableAccess) : base(variableAccess)
    {
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorVariableDictionaryElementNameNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<VariableName>{VariableName}</VariableName>");
        stringBuilder.AppendLine(KeySelector.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorVariableDictionaryElementNameNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService, Location location)
    {
        // TODO - No filtering for dictionary element variable name selector yet built
        throw new NotImplementedException("No filtering for dictionary element variable name selector yet built");
    }
}