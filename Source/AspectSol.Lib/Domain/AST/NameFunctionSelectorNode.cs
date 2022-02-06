using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class NameFunctionSelectorNode : FunctionSelectorNode
{
    public string FunctionName { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<NameFunctionSelectorNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<FunctionName>{FunctionName}</FunctionName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</NameFunctionSelectorNode>");

        return stringBuilder.ToString();
    }
}