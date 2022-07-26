using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SelectorVariablePropertyNameNode : SelectorNode
{
    public string PropertyName { get; init; }

    public SelectorVariablePropertyNameNode Child { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorVariablePropertyNameNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<PropertyName>{PropertyName}</PropertyName>");

        if (Child != null)
        {
            stringBuilder.AppendLine($"{GetIndentation()}{Child}");
        }

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorVariablePropertyNameNode)}>");

        return stringBuilder.ToString();
    }

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        // TODO - No filtering for name property selector yet built
        throw new NotImplementedException("No filtering for name property selector yet built");
    }
}