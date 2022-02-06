using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class ConstantKeySelectorNode : KeySelectorNode
{
    public string Constant { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<ConstantKeySelectorNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Constant>{Constant}</Constant>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</ConstantKeySelectorNode>");

        return stringBuilder.ToString();
    }
}