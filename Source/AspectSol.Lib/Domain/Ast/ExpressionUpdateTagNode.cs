using System.Text;

namespace AspectSol.Lib.Domain.Ast;

public class ExpressionUpdateTagNode : ExpressionNode
{
    public string OldModifier { get; set; }

    public string NewModifier { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(ExpressionUpdateTagNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<OldModifier>{OldModifier}</OldModifier>");
        stringBuilder.AppendLine($"{GetIndentation()}<NewModifier>{NewModifier}</NewModifier>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(ExpressionUpdateTagNode)}>");

        return stringBuilder.ToString();
    }

    public override string GetValue()
    {
        return string.Empty;
    }
}