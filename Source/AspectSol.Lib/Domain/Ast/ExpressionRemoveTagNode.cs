using System.Text;

namespace AspectSol.Lib.Domain.Ast;

public class ExpressionRemoveTagNode : ExpressionNode
{
    public string Modifier { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(ExpressionRemoveTagNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Modifier>{Modifier}</Modifier>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(ExpressionRemoveTagNode)}>");

        return stringBuilder.ToString();
    }

    public override string GetValue()
    {
        return string.Empty;
    }
}