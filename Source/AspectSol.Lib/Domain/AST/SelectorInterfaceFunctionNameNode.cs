using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Infra.Enums;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.AST;

public class SelectorInterfaceFunctionNameNode : SelectorNode
{
    public string InterfaceName { get; init; }
    
    public InterfaceTagNode InterfaceTagNode { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorInterfaceFunctionNameNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<InterfaceName>{InterfaceName}</InterfaceName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorInterfaceFunctionNameNode)}>");

        return stringBuilder.ToString();
    }

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        var invert = InterfaceTagNode.Value == InterfaceTag.NotInInterface;
        
        var selectionResult = abstractFilteringService.FunctionFiltering.FilterFunctionsImplementedFromInterface(smartContract, InterfaceName, invert);
        return selectionResult;
    }
}