using System.Text;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SelectorContractAddressNode : SelectorNode
{
    public string ContractAddress { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SelectorContractAddressNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<ContractAddress>{ContractAddress}</ContractAddress>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SelectorContractAddressNode)}>");

        return stringBuilder.ToString();
    }

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        // TODO - No filtering for smart contract by address yet built
        throw new NotImplementedException("No filtering for smart contract by address yet built");
    }
}