using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class ImplementingDefinitionDecoratorNode : DefinitionDecoratorNode
{
    public string InterfaceName { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<ImplementingDefinitionDecoratorNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine(InterfaceName);

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</ImplementingDefinitionDecoratorNode>");

        return stringBuilder.ToString();
    }
}