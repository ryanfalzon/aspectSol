using AspectSol.Lib.Infra.Enums;
using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.AST;

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

    public override SelectionResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService)
    {
        SelectionResult modifierSelectionResult;
        
        if (Left.GetType() == typeof(ModifierNode))
        {
            var leftModifierName = (Left as ModifierNode)?.ModifierName;

            if (Right is null)
            {
                modifierSelectionResult = Operator switch
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

                modifierSelectionResult = Operator switch
                {
                    ModifierOperator.Or  => abstractFilteringService.FunctionFiltering.FilterFunctionsByEitherModifiers(smartContract, modifiers),
                    ModifierOperator.And => abstractFilteringService.FunctionFiltering.FilterFunctionsByAllModifiers(smartContract, modifiers),
                    _                    => throw new ArgumentOutOfRangeException($"Operator [{Operator}] cannot be applied on less than 2 modifier")
                };
            }
            else
            {
                var leftSelectionResult = abstractFilteringService.FunctionFiltering.FilterFunctionsByModifier(smartContract, leftModifierName, false);
                var rightSelectionResult = Right.Filter(smartContract, abstractFilteringService);

                modifierSelectionResult = Operator switch
                {
                    ModifierOperator.Or => new SelectionResult
                    {
                        InterestedContracts   = leftSelectionResult.InterestedContracts.SafeUnion(rightSelectionResult.InterestedContracts),
                        InterestedFunctions   = leftSelectionResult.InterestedFunctions.SafeUnion(rightSelectionResult.InterestedFunctions),
                        InterestedDefinitions = leftSelectionResult.InterestedDefinitions.SafeUnion(rightSelectionResult.InterestedDefinitions),
                        InterestedStatements  = leftSelectionResult.InterestedStatements.SafeUnion(rightSelectionResult.InterestedStatements)
                    },
                    ModifierOperator.And => new SelectionResult
                    {
                        InterestedContracts   = leftSelectionResult.InterestedContracts.SafetIntersect(rightSelectionResult.InterestedContracts),
                        InterestedFunctions   = leftSelectionResult.InterestedFunctions.SafetIntersect(rightSelectionResult.InterestedFunctions),
                        InterestedDefinitions = leftSelectionResult.InterestedDefinitions.SafetIntersect(rightSelectionResult.InterestedDefinitions),
                        InterestedStatements  = leftSelectionResult.InterestedStatements.SafetIntersect(rightSelectionResult.InterestedStatements)
                    },
                    _ => throw new ArgumentOutOfRangeException($"Operator [{Operator}] cannot be applied on less than 2 modifier")
                };
            }
        }
        else
        {
            if (Right is null)
            {
                modifierSelectionResult = Left.Filter(smartContract, abstractFilteringService);
            }
            else if (Right.GetType() == typeof(ModifierNode))
            {
                var leftSelectionResult = Left.Filter(smartContract, abstractFilteringService);
                var rightSelectionResult = Right.Filter(smartContract, abstractFilteringService);

                modifierSelectionResult = Operator switch
                {
                    ModifierOperator.Or => new SelectionResult
                    {
                        InterestedContracts   = leftSelectionResult.InterestedContracts.SafeUnion(rightSelectionResult.InterestedContracts),
                        InterestedFunctions   = leftSelectionResult.InterestedFunctions.SafeUnion(rightSelectionResult.InterestedFunctions),
                        InterestedDefinitions = leftSelectionResult.InterestedDefinitions.SafeUnion(rightSelectionResult.InterestedDefinitions),
                        InterestedStatements  = leftSelectionResult.InterestedStatements.SafeUnion(rightSelectionResult.InterestedStatements)
                    },
                    ModifierOperator.And => new SelectionResult
                    {
                        InterestedContracts   = leftSelectionResult.InterestedContracts.SafetIntersect(rightSelectionResult.InterestedContracts),
                        InterestedFunctions   = leftSelectionResult.InterestedFunctions.SafetIntersect(rightSelectionResult.InterestedFunctions),
                        InterestedDefinitions = leftSelectionResult.InterestedDefinitions.SafetIntersect(rightSelectionResult.InterestedDefinitions),
                        InterestedStatements  = leftSelectionResult.InterestedStatements.SafetIntersect(rightSelectionResult.InterestedStatements)
                    },
                    _ => throw new ArgumentOutOfRangeException($"Operator [{Operator}] cannot be applied on less than 2 modifier")
                };
            }
            else
            {
                var leftSelectionResult = Left.Filter(smartContract, abstractFilteringService);
                var rightSelectionResult = Right.Filter(smartContract, abstractFilteringService);

                modifierSelectionResult = Operator switch
                {
                    ModifierOperator.Or => new SelectionResult
                    {
                        InterestedContracts   = leftSelectionResult.InterestedContracts.SafeUnion(rightSelectionResult.InterestedContracts),
                        InterestedFunctions   = leftSelectionResult.InterestedFunctions.SafeUnion(rightSelectionResult.InterestedFunctions),
                        InterestedDefinitions = leftSelectionResult.InterestedDefinitions.SafeUnion(rightSelectionResult.InterestedDefinitions),
                        InterestedStatements  = leftSelectionResult.InterestedStatements.SafeUnion(rightSelectionResult.InterestedStatements)
                    },
                    ModifierOperator.And => new SelectionResult
                    {
                        InterestedContracts   = leftSelectionResult.InterestedContracts.SafetIntersect(rightSelectionResult.InterestedContracts),
                        InterestedFunctions   = leftSelectionResult.InterestedFunctions.SafetIntersect(rightSelectionResult.InterestedFunctions),
                        InterestedDefinitions = leftSelectionResult.InterestedDefinitions.SafetIntersect(rightSelectionResult.InterestedDefinitions),
                        InterestedStatements  = leftSelectionResult.InterestedStatements.SafetIntersect(rightSelectionResult.InterestedStatements)
                    },
                    _ => throw new ArgumentOutOfRangeException($"Operator [{Operator}] cannot be applied on less than 2 modifier")
                };
            }
        }

        return modifierSelectionResult;
    }
}