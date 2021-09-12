using AspectSol.Compiler.Infra.Enums;
using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class VariableDecoratorNode : DecoratorNode
    {
        public VariableVisibility VariableVisibility { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<VariableDecoratorNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine($"{GetIndentation()}<VariableVisibility>{VariableVisibility}</VariableVisibility>");

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</VariableDecoratorNode>");

            return stringBuilder.ToString();
        }
    }
}