using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class WildcardSelectorNode : SelectorNode
    {
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<WildcardSelectorNode></WildcardSelectorNode>");

            return stringBuilder.ToString();
        }
    }
}