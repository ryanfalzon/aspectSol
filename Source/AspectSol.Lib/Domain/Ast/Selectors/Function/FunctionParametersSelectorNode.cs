using System.Text;

namespace AspectSol.Lib.Domain.Ast.Selectors.Function;

public class FunctionParametersSelectorNode : FunctionNameSelectorNode
{
    public List<ParameterNode> Parameters { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(FunctionNameSelectorNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<FunctionName>{FunctionName}</FunctionName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(FunctionNameSelectorNode)}>");

        return stringBuilder.ToString();
    }
}