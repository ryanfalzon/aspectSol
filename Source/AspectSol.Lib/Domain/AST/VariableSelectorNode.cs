using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class VariableSelectorNode : SelectorNode
{
    public VariableSyntaxNode VariableSyntaxNode { get; set; }

    public SelectorNode VariableType { get; set; }

    public SelectorNode VariableLocation { get; set; }

    public SelectorNode VariableNameSelector { get; set; }

    public VariableDecoratorNode VariableDecorator { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<VariableSelectorNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine(VariableSyntaxNode.ToString());
        stringBuilder.AppendLine(VariableType.ToString());
        stringBuilder.AppendLine(VariableLocation.ToString());
        stringBuilder.AppendLine(VariableNameSelector.ToString());
        stringBuilder.AppendLine(VariableDecorator.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</VariableSelectorNode>");

        return stringBuilder.ToString();
    }
}