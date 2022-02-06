using AspectSol.Lib.Infra.Enums;
using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class ModifierSyntaxNode : SyntaxNode
{
    public ModifierOperator Operator { get; set; }

    public ModifierSyntaxNode Left { get; set; }

    public ModifierSyntaxNode Right { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<ModifierSyntaxNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Operator>{Operator}</Operator>");
        stringBuilder.AppendLine(Left.ToString());
        stringBuilder.AppendLine(Right.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</ModifierSyntaxNode>");

        return stringBuilder.ToString();
    }
}