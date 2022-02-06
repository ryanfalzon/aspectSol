using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class ReferenceDefinitionSyntaxNode : DefinitionSyntaxNode
{
    public SelectorNode ContractSelector { get; set; }

    public SelectorNode FunctionSelector { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<ReferenceDefinitionSyntaxNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine(ContractSelector.ToString());
        stringBuilder.AppendLine(FunctionSelector.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</ReferenceDefinitionSyntaxNode>");

        return stringBuilder.ToString();
    }
}