using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class NamePropertySelectorNode : PropertySelectorNode
{
    public string PropertyName { get; set; }

    public NamePropertySelectorNode Child { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<NamePropertySelectorNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<PropertyName>{PropertyName}</PropertyName>");

        if (Child != null)
        {
            stringBuilder.AppendLine($"{GetIndentation()}<NamePropertySelectorNode>{Child}</NamePropertySelectorNode>");
        }

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</NamePropertySelectorNode>");

        return stringBuilder.ToString();
    }
}