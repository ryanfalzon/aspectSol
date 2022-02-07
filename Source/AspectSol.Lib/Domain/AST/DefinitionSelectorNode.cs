using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class DefinitionSelectorNode : SelectorNode
{
    public DefinitionSyntaxNode DefinitionSyntax { get; set; }

    public List<ParameterNode> Parameters { get; set; }

    public DefinitionDecoratorNode DefinitionDecorator { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<DefinitionSelectorNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine(DefinitionSyntax.ToString());
        stringBuilder.AppendLine(string.Join('\n', Parameters));
        stringBuilder.AppendLine(DefinitionDecorator.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</DefinitionSelectorNode>");

        return stringBuilder.ToString();
    }
}