using System.Text;

namespace AspectSol.Lib.Domain.Ast;

public class SenderNode : Node
{
    public SyntaxDefinitionNodeReference SyntaxDefinitionNodeReference { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SenderNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(SyntaxDefinitionNodeReference.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SenderNode)}>");

        return stringBuilder.ToString();
    }
}