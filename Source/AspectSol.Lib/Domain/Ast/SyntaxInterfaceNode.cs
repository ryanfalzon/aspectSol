using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
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

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService, Location location)
    {
        var selectionResult = InterfaceSelector.Filter(smartContract, abstractFilteringService, location);
        return selectionResult;
    }
}