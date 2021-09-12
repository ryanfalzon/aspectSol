using AspectSol.Compiler.Infra.Enums;
using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class ModifierSyntaxNode : SyntaxNode
    {
        public ModifierOperator Operator { get; set; }

        public ModifierNode Left { get; set; }

        public ModifierNode Right { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

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
}