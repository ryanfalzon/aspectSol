using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class NameContractSelectorNode : ContractSelectorNode
{
    public string ContractName { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<NameContractSelectorNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<ContractName>{ContractName}</ContractName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</NameContractSelectorNode>");

        return stringBuilder.ToString();
    }
}