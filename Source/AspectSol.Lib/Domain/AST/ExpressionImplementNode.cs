using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class ExpressionImplementNode : ExpressionNode
{
    public string InterfaceName { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(ExpressionImplementNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<InterfaceName>{InterfaceName}</InterfaceName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(ExpressionImplementNode)}>");

        return stringBuilder.ToString();
    }
}