﻿using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class AddressContractSelectorNode : SelectorNode
{
    public string ContractAddress { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<AddressContractSelectorNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<ContractAddress>{ContractAddress}</ContractAddress>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</AddressContractSelectorNode>");

        return stringBuilder.ToString();
    }
}