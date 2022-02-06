﻿using AspectSol.Lib.Infra.Enums;
using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class ModificationTypeNode : Node
{
    public ModificationType Value { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<ModificationTypeNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Value>{Value}</Value>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</ModificationTypeNode>");

        return stringBuilder.ToString();
    }
}