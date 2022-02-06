using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class TaggedDefinitionDecoratorNode : DefinitionDecoratorNode
{
    public ModifierSyntaxNode ModifierSyntax { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<TaggedDefinitionDecoratorNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine(ModifierSyntax.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</TaggedDefinitionDecoratorNode>");

        return stringBuilder.ToString();
    }
}