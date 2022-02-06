using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class InterfaceContractSelectorNode : ContractSelectorNode
{
    public string ContractName { get; set; }

    public string InterfaceName { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<InterfaceContractSelectorNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<ContractName>{ContractName}</ContractName>");
        stringBuilder.AppendLine($"{GetIndentation()}<InterfaceName>{InterfaceName}</InterfaceName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</InterfaceContractSelectorNode>");

        return stringBuilder.ToString();
    }
}