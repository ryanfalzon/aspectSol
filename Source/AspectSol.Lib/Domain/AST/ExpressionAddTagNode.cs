using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class ExpressionAddTagNode : ExpressionNode
{
    public string Modifier { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(ExpressionAddTagNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Modifier>{Modifier}</Modifier>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(ExpressionAddTagNode)}>");

        return stringBuilder.ToString();
    }
}