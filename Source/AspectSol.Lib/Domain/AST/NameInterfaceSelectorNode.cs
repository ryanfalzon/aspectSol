using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class NameInterfaceSelectorNode : InterfaceSelectorNode
{
    public string InterfaceName { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<NameInterfaceSelectorNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<InterfaceName>{InterfaceName}</InterfaceName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</NameInterfaceSelectorNode>");

        return stringBuilder.ToString();
    }
}