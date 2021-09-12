using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class TypeVariableTypeSelectorNode : VariableTypeSelectorNode
    {
        public string Type { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<TypeVariableTypeSelectorNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine($"{GetIndentation()}<Type>{Type}</Type>");

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</TypeVariableTypeSelectorNode>");

            return stringBuilder.ToString();
        }
    }
}