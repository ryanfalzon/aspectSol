using System.Text;
using AspectSol.Lib.Domain.Ast.Modifications;
using AspectSol.Lib.Domain.Ast.Syntax;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.JavascriptExecution;
using AspectSol.Lib.Infra.Enums;
using AspectSol.Lib.Infra.Helpers;
using AspectSol.Lib.Infra.Helpers.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Statements;

public class ModificationStatementNode : StatementNode
{
    public ModificationNode Modification { get; init; }
    public ReferenceSyntaxDefinitionNode ReferenceSyntaxDefinitionNodeNode { get; init; }
    public DecoratorDefinitionNode DecoratorDefinition { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(ModificationStatementNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(Modification.ToString());
        stringBuilder.AppendLine(ReferenceSyntaxDefinitionNodeNode.ToString());
        stringBuilder.AppendLine(DecoratorDefinition.ToString());
        stringBuilder.AppendLine(base.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(ModificationStatementNode)}>");

        return stringBuilder.ToString();
    }

    public override JToken Execute(JToken smartContract, AbstractFilteringService abstractFilteringService, IJavascriptExecutor javascriptExecutor,
        TempStorageRepository tempStorageRepository)
    {
        var syntaxDefinitionNodeReferenceResult = ReferenceSyntaxDefinitionNodeNode.Filter(smartContract, abstractFilteringService, Location.ExecutionOf);
        var decoratorDefinitionResult = DecoratorDefinition?.Filter(smartContract, abstractFilteringService, Location.ExecutionOf);

        var intersectionResult = FilteringResultHelpers.Intersect(syntaxDefinitionNodeReferenceResult, decoratorDefinitionResult);
        var encodedBody = EncodeBodyContent(intersectionResult, tempStorageRepository, javascriptExecutor);
        
        return Modification.Evaluate(smartContract, intersectionResult, encodedBody, abstractFilteringService);
    }
}