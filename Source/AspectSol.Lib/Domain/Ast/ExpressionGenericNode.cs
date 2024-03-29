﻿using System.Text;

namespace AspectSol.Lib.Domain.Ast;

public class ExpressionGenericNode : ExpressionNode
{
    public string Expression { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(ExpressionGenericNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Expression>");
        IncreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}{Expression}");
        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</Expression>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(ExpressionGenericNode)}>");

        return stringBuilder.ToString();
    }

    public override string GetValue()
    {
        return Expression;
    }
}