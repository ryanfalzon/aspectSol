using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class InstanceDefinitionSyntaxNode : DefinitionSyntaxNode
{
    public NameContractSelectorNode NameContractSelector { get; set; }

    public SelectorNode InstanceSelector { get; set; }

    public SelectorNode FunctionSelector { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<InstanceDefinitionSyntaxNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine(NameContractSelector.ToString());
        stringBuilder.AppendLine(InstanceSelector.ToString());
        stringBuilder.AppendLine(FunctionSelector.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</InstanceDefinitionSyntaxNode>");

        return stringBuilder.ToString();
    }
}