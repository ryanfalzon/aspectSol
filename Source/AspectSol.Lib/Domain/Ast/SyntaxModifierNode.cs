using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Enums;
using AspectSol.Lib.Infra.Helpers.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public class SyntaxModifierNode : SyntaxNode
{
    public ModifierOperator Operator { get; set; }

    public SyntaxModifierNode Left { get; set; }

    public SyntaxModifierNode Right { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(SyntaxModifierNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Operator>{Operator}</Operator>");
        stringBuilder.AppendLine(Left.ToString());
        stringBuilder.AppendLine(Right.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(SyntaxModifierNode)}>");

        return stringBuilder.ToString();
    }

    public override FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        FilteringResult modifierFilteringResult;
        
        if (Left.GetType() == typeof(ModifierNode))
        {
            var leftModifierName = (Left as ModifierNode)?.ModifierName;

            if (Right is null)
            {
                modifierFilteringResult = Operator switch
                {
                    ModifierOperator.Not  => abstractFilteringService.FunctionFiltering.FilterFunctionsByModifier(smartContract, leftModifierName, true),
                    ModifierOperator.None => abstractFilteringService.FunctionFiltering.FilterFunctionsByModifier(smartContract, leftModifierName, false),
                    _                     => throw new ArgumentOutOfRangeException($"Operator [{Operator}] cannot be applied on more than 1 modifier")
                };
            }
            else if (Right.GetType() == typeof(ModifierNode))
            {
                var modifiers = new List<string>
                {
                    leftModifierName,
                    (Right as ModifierNode)?.ModifierName
                };

                modifierFilteringResult = Operator switch
                {
                    ModifierOperator.Or  => abstractFilteringService.FunctionFiltering.FilterFunctionsByEitherModifiers(smartContract, modifiers),
                    ModifierOperator.And => abstractFilteringService.FunctionFiltering.FilterFunctionsByAllModifiers(smartContract, modifiers),
                    _                    => throw new ArgumentOutOfRangeException($"Operator [{Operator}] cannot be applied on less than 2 modifier")
                };
            }
            else
            {
                var leftFilteringResult = abstractFilteringService.FunctionFiltering.FilterFunctionsByModifier(smartContract, leftModifierName, false);
                var rightFilteringResult = Right.Filter(smartContract, abstractFilteringService);

                modifierFilteringResult = Operator switch
                {
                    ModifierOperator.Or  => FilteringResultHelpers.Union(leftFilteringResult, rightFilteringResult),
                    ModifierOperator.And => FilteringResultHelpers.Intersect(leftFilteringResult, rightFilteringResult),
                    _                    => throw new ArgumentOutOfRangeException($"Operator [{Operator}] cannot be applied on less than 2 modifier")
                };
            }
        }
        else
        {
            if (Right is null)
            {
                modifierFilteringResult = Left.Filter(smartContract, abstractFilteringService);
            }
            else if (Right.GetType() == typeof(ModifierNode))
            {
                var leftFilteringResult = Left.Filter(smartContract, abstractFilteringService);
                var rightFilteringResult = Right.Filter(smartContract, abstractFilteringService);

                modifierFilteringResult = Operator switch
                {
                    ModifierOperator.Or  => FilteringResultHelpers.Union(leftFilteringResult, rightFilteringResult),
                    ModifierOperator.And => FilteringResultHelpers.Intersect(leftFilteringResult, rightFilteringResult),
                    _                    => throw new ArgumentOutOfRangeException($"Operator [{Operator}] cannot be applied on less than 2 modifier")
                };
            }
            else
            {
                var leftFilteringResult = Left.Filter(smartContract, abstractFilteringService);
                var rightFilteringResult = Right.Filter(smartContract, abstractFilteringService);

                modifierFilteringResult = Operator switch
                {
                    ModifierOperator.Or  => FilteringResultHelpers.Union(leftFilteringResult, rightFilteringResult),
                    ModifierOperator.And => FilteringResultHelpers.Intersect(leftFilteringResult, rightFilteringResult),
                    _                    => throw new ArgumentOutOfRangeException($"Operator [{Operator}] cannot be applied on less than 2 modifier")
                };
            }
        }

        return modifierFilteringResult;
    }
}