using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class AspectUpdateTagExpressionNode : AspectExpressionNode
{
    public string OldModifier { get; set; }

    public string NewModifier { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<AspectUpdateTagExpressionNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<OldModifier>{OldModifier}</OldModifier>");
        stringBuilder.AppendLine($"{GetIndentation()}<NewModifier>{NewModifier}</NewModifier>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</AspectUpdateTagExpressionNode>");

        return stringBuilder.ToString();
    }
}