using System.Text;
using AspectSol.Lib.Domain.Filtering;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.AST;

public class SelectorVariableConstantKeyNode : SelectorKeyNode
{
    public string Constant { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorVariableConstantKeyNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Constant>{Constant}</Constant>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorVariableConstantKeyNode)}>");

        return stringBuilder.ToString();
    }

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        // TODO - No filtering for constant key selector yet built
        throw new NotImplementedException("No filtering for constant key selector yet built");
    }
}