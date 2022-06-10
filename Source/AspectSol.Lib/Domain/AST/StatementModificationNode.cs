using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.JavascriptExecution;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.AST;

public class StatementModificationNode : StatementNode
{
    public ModificationTypeNode ModificationType { get; set; }

    public SyntaxDefinitionNodeReference SyntaxDefinitionNodeReferenceDefinitionNode { get; set; }

    public DecoratorDefinitionNode DecoratorDefinition { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(StatementModificationNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(ModificationType.ToString());
        stringBuilder.AppendLine(SyntaxDefinitionNodeReferenceDefinitionNode.ToString());
        stringBuilder.AppendLine(DecoratorDefinition.ToString());
        stringBuilder.AppendLine(base.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(StatementModificationNode)}>");

        return stringBuilder.ToString();
    }

    public override JToken Execute(JToken smartContract, AbstractFilteringService abstractFilteringService, IJavascriptExecutor javascriptExecutor)
    {
        // TODO - No executor for statement modification node yet built
        throw new NotImplementedException("No executor for statement modification node yet built");
    }
}