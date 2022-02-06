using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class AspectImplementExpressionNode : AspectExpressionNode
{
    public string InterfaceName { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<AspectImplementExpressionNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<InterfaceName>{InterfaceName}</InterfaceName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</AspectImplementExpressionNode>");

        return stringBuilder.ToString();
    }
}