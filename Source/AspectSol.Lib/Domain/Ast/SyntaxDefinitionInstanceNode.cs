using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SyntaxDefinitionInstanceNode : SyntaxDefinitionNode
{
    public SelectorContractNameNode SelectorContractNameNodeContractSelector { get; set; }

    public SelectorNode InstanceSelector { get; set; }

    public SelectorNode FunctionSelector { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SyntaxDefinitionInstanceNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(SelectorContractNameNodeContractSelector.ToString());
        stringBuilder.AppendLine(InstanceSelector.ToString());
        stringBuilder.AppendLine(FunctionSelector.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SyntaxDefinitionInstanceNode)}>");

        return stringBuilder.ToString();
    }

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        // TODO - No filtering for instance definition syntax node yet built
        throw new NotImplementedException("No filtering for instance definition syntax node yet built");
    }
}