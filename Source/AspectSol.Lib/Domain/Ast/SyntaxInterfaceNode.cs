using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SyntaxInterfaceNode : SelectorNode
{
    public InterfaceTagNode InterfaceTagNode { get; init; }

    public SelectorNode InterfaceSelector { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SyntaxInterfaceNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(InterfaceTagNode.ToString());
        stringBuilder.AppendLine(InterfaceSelector.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SyntaxInterfaceNode)}>");

        return stringBuilder.ToString();
    }

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var selectionResult = InterfaceSelector.Filter(smartContract, abstractFilteringService);
        return selectionResult;
    }
}