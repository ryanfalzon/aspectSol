using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class ModifierNode : ModifierSyntaxNode
{
    public string ModifierName { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<ModifierNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<ModifierName>{ModifierName}</ModifierName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</ModifierNode>");

        return stringBuilder.ToString();
    }
}