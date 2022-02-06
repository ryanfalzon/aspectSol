using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class SenderNode : Node
{
    public ReferenceDefinitionSyntaxNode ReferenceDefinition { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<SenderNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine(ReferenceDefinition.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</SenderNode>");

        return stringBuilder.ToString();
    }
}