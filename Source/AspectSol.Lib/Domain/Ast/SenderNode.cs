using System.Text;
using AspectSol.Lib.Domain.Ast.Syntax;

namespace AspectSol.Lib.Domain.Ast;

public class SenderNode : Node
{
    public ReferenceSyntaxDefinitionNode ReferenceSyntaxDefinitionNode { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SenderNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(ReferenceSyntaxDefinitionNode.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SenderNode)}>");

        return stringBuilder.ToString();
    }
}