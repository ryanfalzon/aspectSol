using System.Text;
using AspectSol.Lib.Domain.Ast.Modifications;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.JavascriptExecution;
using AspectSol.Lib.Infra.Helpers;
using AspectSol.Lib.Infra.TemporaryStorage;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Statements;

public class ModificationStatementNode : StatementNode
{
    public ModificationNode Modification { get; init; }
    public SyntaxDefinitionNodeReference SyntaxDefinitionNodeReferenceNode { get; init; }
    public DecoratorDefinitionNode DecoratorDefinition { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(ModificationStatementNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(Modification.ToString());
        stringBuilder.AppendLine(SyntaxDefinitionNodeReferenceNode.ToString());
        stringBuilder.AppendLine(DecoratorDefinition.ToString());
        stringBuilder.AppendLine(base.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(ModificationStatementNode)}>");

        return stringBuilder.ToString();
    }

    public override JToken Execute(JToken smartContract, AbstractFilteringService abstractFilteringService, IJavascriptExecutor javascriptExecutor,
        TempStorageRepository tempStorageRepository, SolidityAstNodeIdResolver solidityAstNodeIdResolver)
    {
        var syntaxDefinitionNodeReferenceResult = SyntaxDefinitionNodeReferenceNode.Filter(smartContract, abstractFilteringService);
        var decoratorDefinitionResult = DecoratorDefinition?.Filter(smartContract, abstractFilteringService);

        var intersectionResult = SelectionResultHelpers.Intersect(syntaxDefinitionNodeReferenceResult, decoratorDefinitionResult);
        var encodedBody = EncodeBodyContent(intersectionResult, tempStorageRepository, javascriptExecutor, solidityAstNodeIdResolver);
        
        return Modification.Evaluate(smartContract, intersectionResult, encodedBody, abstractFilteringService);
    }
}