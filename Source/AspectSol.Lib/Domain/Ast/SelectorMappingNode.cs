using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SelectorMappingNode : SelectorNode
{
    public string MappingName { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorMappingNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<MappingName>{MappingName}</MappingName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorMappingNode)}>");

        return stringBuilder.ToString();
    }

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        // TODO - No filtering for mapping selector yet built
        throw new NotImplementedException("No filtering for mapping selector yet built");
    }
}