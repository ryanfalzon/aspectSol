using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Infra.Enums;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.AST;

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

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        // TODO - No filtering for dictionary element variable name selector yet built
        throw new NotImplementedException("No filtering for dictionary element variable name selector yet built");
    }
}