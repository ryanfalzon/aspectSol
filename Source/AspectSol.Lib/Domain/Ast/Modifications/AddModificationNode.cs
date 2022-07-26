using System.Text;
using AspectSol.Lib.Domain.Filtering;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Modifications;

public class AddModificationNode : ModificationNode
{
    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(AddModificationNode)}>");
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(AddModificationNode)}>");

        return stringBuilder.ToString();
    }

    public override JToken Evaluate(JToken contract, SelectionResult selectionResult, JToken content, AbstractFilteringService abstractFilteringService)
    {
        if (selectionResult.InterestedFunctions != null && selectionResult.InterestedFunctions.Count != 0)
        {
            abstractFilteringService.SourceManipulation.AddDefinitionToFunction(ref contract, content, selectionResult);
            return contract;
        }

        if (selectionResult.InterestedContracts != null && selectionResult.InterestedContracts.Count != 0)
        {
            abstractFilteringService.SourceManipulation.AddDefinitionToContract(ref contract, content, selectionResult);
            return contract;
        }

        return contract;
    }
}