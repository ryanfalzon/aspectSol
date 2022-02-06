using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class WildcardSelectorNode : SelectorNode
{
    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<WildcardSelectorNode></WildcardSelectorNode>");

        return stringBuilder.ToString();
    }
}