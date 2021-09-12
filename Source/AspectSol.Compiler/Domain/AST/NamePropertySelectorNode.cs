using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class NamePropertySelectorNode : PropertySelectorNode
    {
        public string PropertyName { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<NamePropertySelectorNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine($"{GetIndentation()}<PropertyName>{PropertyName}</PropertyName>");

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</NamePropertySelectorNode>");

            return stringBuilder.ToString();
        }
    }
}