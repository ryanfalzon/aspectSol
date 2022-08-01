using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Syntax;

public class InstanceSyntaxDefinitionNode : SyntaxDefinitionNode
{
    public SelectorContractNameNode SelectorContractNameNodeContractSelector { get; init; }
    public SelectorNode InstanceSelector { get; init; }
    public SelectorNode FunctionSelector { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(InstanceSyntaxDefinitionNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(SelectorContractNameNodeContractSelector.ToString());
        stringBuilder.AppendLine(InstanceSelector.ToString());
        stringBuilder.AppendLine(FunctionSelector.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(InstanceSyntaxDefinitionNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        // TODO - No filtering for instance syntax definition node yet built
        throw new NotImplementedException("No filtering for instance syntax definition node yet built");
    }
}