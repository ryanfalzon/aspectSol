using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class AspectAddTagExpressionNode : AspectExpressionNode
    {
        public string Modifier { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<AspectAddTagExpressionNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine($"{GetIndentation()}<Modifier>{Modifier}</Modifier>");

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</AspectAddTagExpressionNode>");

            return stringBuilder.ToString();
        }
    }
}