using System.Text;

namespace AspectSol.Lib.Domain.Ast;

public class ModifierNode : SyntaxModifierNode
{
    public string ModifierName { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(ModifierNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<ModifierName>{ModifierName}</ModifierName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(ModifierNode)}>");

        return stringBuilder.ToString();
    }
}