using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

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

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService, Location location)
    {
        // TODO - No filtering for constant key selector yet built
        throw new NotImplementedException("No filtering for constant key selector yet built");
    }
}