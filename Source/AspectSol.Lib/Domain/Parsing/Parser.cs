using AspectSol.Lib.Domain.AST;
using AspectSol.Lib.Domain.Tokenization;
using AspectSol.Lib.Infra.Enums;
using AspectSol.Lib.Infra.Exceptions;
using AspectSol.Lib.Infra.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace AspectSol.Lib.Domain.Parsing;

public class Parser : AbstractParser
{
    public override AspectNode Parse(List<DslToken> tokens)
    {
        Initialize(tokens);

        var aspectNode = Match();

        DiscardToken(TokenType.SequenceTerminator);

        return aspectNode;
    }

    private AspectNode Match()
    {
        AspectNode node = null;
        if (LookaheadFirst.TokenType == TokenType.Aspect)
        {
            DiscardToken();
            ValidateToken(TokenType.StringValue);

            node = new AspectNode
            {
                Name = LookaheadFirst.Value,
                Statements = new List<StatementNode>()
            };

            DiscardToken(TokenType.OpenScope);

            while (LookaheadFirst.TokenType != TokenType.SequenceTerminator)
            {
                node.Statements.Add(MatchStatement());
            }

            DiscardToken(TokenType.CloseScope);
        }
        else
        {
            // TODO - Throw error is parse was not successful
        }

        return node;
    }

    private StatementNode MatchStatement()
    {
        StatementNode node;

        if (LookaheadFirst.IsAppendStatement())
        {
            node = MatchAppendStatement();
        }
        else if (LookaheadFirst.IsModificationStatement())
        {
            node = MatchModificationStatement();
        }
        else
        {
            throw new DslParserException("");
        }

        DiscardToken(TokenType.OpenScope);
        DiscardToken(TokenType.CloseScope);

        return node;
    }

    private AppendStatementNode MatchAppendStatement()
    {
        var appendStatement = new AppendStatementNode
        {
            Placement = new PlacementNode { Value = LookaheadFirst.GetPlacement() }
        };

        DiscardToken();

        appendStatement.Location = new LocationNode { Value = LookaheadFirst.GetLocation() };
        DiscardToken();

        var selectors = new List<SelectorNode>();
        if (LookaheadFirst.IsDefinitionSelector())
        {
            selectors.Add(MatchDefinitionSelector());
            selectors.AddRange(MatchReturnSelector());
            selectors.AddRange(MatchInterfaceSelector());
        }
        else if (LookaheadFirst.IsVariableSelector())
        {
            selectors.Add(MatchVariableSelector());
        }
        else
        {
            throw new DslParserException("");
        }

        appendStatement.Selectors = selectors;

        if (LookaheadFirst.TokenType == TokenType.OriginatingFrom)
        {
            DiscardToken();

            appendStatement.Sender = new SenderNode
            {
                ReferenceDefinition = MatchReferenceDefinition()
            };
        }

        return appendStatement;
    }

    private DefinitionSelectorNode MatchDefinitionSelector()
    {
        var definitionSelector = new DefinitionSelectorNode();

        // Step 1 - Parse definition syntax
        // TODO - Support InstanceDefinitionSyntax
        definitionSelector.DefinitionSyntax = MatchReferenceDefinition();

        // Step 2 - Parse optional parameters
        definitionSelector.Parameters = MatchOptionalParameterNodes().ToList();

        // Step 3 - Parse definition decorator
        if (LookaheadFirst.TokenType == TokenType.TaggedWith)
        {
            DiscardToken();
            definitionSelector.DefinitionDecorator = new TaggedDefinitionDecoratorNode
            {
                ModifierSyntax = MatchModifierSyntax()
            };
        }
        else if (LookaheadFirst.TokenType == TokenType.ImplementingInterface && LookaheadSecond.TokenType == TokenType.StringValue)
        {
            DiscardToken();
            definitionSelector.DefinitionDecorator = new ImplementingDefinitionDecoratorNode
            {
                InterfaceName = LookaheadFirst.Value
            };
        }
        else
        {
            // TODO - Throw error is parse was not successful
        }

        return definitionSelector;
    }

    private ReferenceDefinitionSyntaxNode MatchReferenceDefinition()
    {
        var definitionSyntax = new ReferenceDefinitionSyntaxNode
        {
            ContractSelector = MatchContractSelector()
        };

        DiscardToken(TokenType.FullStop);
        if (LookaheadFirst.TokenType == TokenType.Wildcard)
        {
            definitionSyntax.FunctionSelector = new WildcardSelectorNode();
            DiscardToken();
        }
        else if (LookaheadFirst.TokenType == TokenType.StringValue)
        {
            definitionSyntax.FunctionSelector = new NameContractSelectorNode { ContractName = LookaheadFirst.Value };
            DiscardToken();
        }
        else
        {
            // TODO - Throw error is parse was not successful
        }

        return definitionSyntax;
    }

    private SelectorNode MatchContractSelector()
    {
        SelectorNode selector = null;

        if (LookaheadFirst.TokenType == TokenType.Wildcard)
        {
            selector = new WildcardSelectorNode();
            DiscardToken();
        }
        else if (LookaheadFirst.TokenType == TokenType.OpenDoubleSquareBrackets)
        {
            DiscardToken();
            ValidateToken(TokenType.StringValue);

            selector = new MappingSelectorNode { MappingName = LookaheadFirst.Value };
            DiscardToken();

            DiscardToken(TokenType.CloseDoubleSquareBrackets);
        }
        else if (LookaheadFirst.TokenType == TokenType.Address)
        {
            selector = new AddressContractSelectorNode { ContractAddress = LookaheadFirst.Value };
            DiscardToken();
        }
        else if (LookaheadFirst.TokenType == TokenType.StringValue &&
                 LookaheadSecond.TokenType == TokenType.DoubleColon)
        {
            var contractName = LookaheadFirst.Value;

            DiscardToken();
            DiscardToken();
            ValidateToken(TokenType.StringValue);

            var interfaceName = LookaheadFirst.Value;

            selector = new InterfaceContractSelectorNode
            {
                ContractName = contractName,
                InterfaceName = interfaceName
            };

            DiscardToken();
        }
        else if (LookaheadFirst.TokenType == TokenType.StringValue)
        {
            selector = new NameContractSelectorNode { ContractName = LookaheadFirst.Value };
            DiscardToken();
        }
        else
        {
            // TODO - Throw error is parse was not successful
        }

        return selector;
    }

    private IEnumerable<ParameterNode> MatchOptionalParameterNodes()
    {
        if (LookaheadFirst.TokenType != TokenType.OpenParenthesis) return null;

        var parameters = new List<ParameterNode>();

        DiscardToken();
        while (LookaheadFirst.TokenType != TokenType.CloseParenthesis)
        {
            if (LookaheadFirst.TokenType == TokenType.StringValue && LookaheadSecond.TokenType == TokenType.StringValue)
            {
                parameters.Add(new ParameterNode
                {
                    Type = LookaheadFirst.Value,
                    Name = LookaheadSecond.Value
                });

                DiscardToken();
                DiscardToken();
            }

            if (LookaheadFirst.TokenType != TokenType.Comma &&
                LookaheadFirst.TokenType != TokenType.CloseParenthesis)
            {
                // TODO - Throw error is parse was not successful
            }
        }

        DiscardToken();

        return parameters;

    }

    private ModifierSyntaxNode MatchModifierSyntax()
    {
        var modifierSyntax = new ModifierSyntaxNode();

        if (LookaheadFirst.TokenType == TokenType.OpenParenthesis)
        {
            DiscardToken();
            modifierSyntax.Left = MatchModifierSyntax();
        }
        else if (LookaheadFirst.TokenType == TokenType.NotSymbol)
        {
            DiscardToken();
            modifierSyntax.Operator = ModifierOperator.Not;
            modifierSyntax.Left = MatchModifierSyntax();
        }
        else if (LookaheadFirst.IsModifierOrVisibility() &&
                 LookaheadSecond.TokenType is TokenType.OrSymbol or TokenType.AndSymbol)
        {
            modifierSyntax.Left = new ModifierNode { ModifierName = LookaheadFirst.Value };
            DiscardToken();

            modifierSyntax.Operator = LookaheadFirst.TokenType == TokenType.AndSymbol
                ? ModifierOperator.And
                : ModifierOperator.Or;
            DiscardToken();

            modifierSyntax.Right = MatchModifierSyntax();
        }
        else if (LookaheadFirst.IsModifierOrVisibility())
        {
            modifierSyntax.Left = new ModifierNode { ModifierName = LookaheadFirst.Value };
            DiscardToken();
        }
        else
        {
            // TODO - Throw error is parse was not successful
        }

        return modifierSyntax;
    }

    private IEnumerable<SelectorNode> MatchReturnSelector()
    {
        var selectors = new List<SelectorNode>();

        if (LookaheadFirst.TokenType == TokenType.ReturningTypes)
        {
            DiscardToken();

            if (LookaheadFirst.TokenType == TokenType.OpenParenthesis)
            {
                var returnSelector = new ReturnSelectorNode
                {
                    ReturnTypes = new List<ReturnTypeNode>()
                };

                DiscardToken();
                while (LookaheadFirst.TokenType != TokenType.CloseParenthesis)
                {
                    if (LookaheadFirst.TokenType == TokenType.StringValue)
                    {
                        returnSelector.ReturnTypes.Add(new ReturnTypeNode
                        {
                            Type = LookaheadFirst.Value
                        });

                        DiscardToken();
                    }

                    if (LookaheadFirst.TokenType != TokenType.Comma &&
                        LookaheadFirst.TokenType != TokenType.CloseParenthesis)
                    {
                        // TODO - Throw error is parse was not successful
                    }
                }

                DiscardToken();

                selectors.Add(returnSelector);
            }
            else
            {
                // TODO - Throw error is parse was not successful
            }
        }

        selectors.AddRange(MatchInterfaceSelector());

        return selectors;
    }

    private IEnumerable<SelectorNode> MatchInterfaceSelector()
    {
        var selectors = new List<SelectorNode>();

        if (LookaheadFirst.TokenType is TokenType.InInterface or TokenType.NotInInterface)
        {
            if (LookaheadSecond.TokenType == TokenType.StringValue)
            {
                var interfaceSelector = new InterfaceSyntaxNode
                {
                    InterfaceNode = new InterfaceNode
                    {
                        Value = LookaheadFirst.TokenType == TokenType.InInterface
                            ? Interface.InInterface
                            : Interface.NotInInterface
                    },
                    InterfaceSelector = new NameInterfaceSelectorNode
                    {
                        InterfaceName = LookaheadSecond.Value
                    }
                };

                DiscardToken();
                DiscardToken();

                selectors.Add(interfaceSelector);
            }
            else
            {
                // TODO - Throw error is parse was not successful
            }
        }

        selectors.AddRange(MatchReturnSelector());

        return selectors;
    }

    private VariableSelectorNode MatchVariableSelector()
    {
        // Step 1 - Parse variable syntax
        var variableSelector = new VariableSelectorNode
        {
            VariableSyntaxNode = new VariableSyntaxNode { Value = LookaheadFirst.GetVariableAccess() }
        };

        DiscardToken();

        // Step 2 - Parse variable type
        if (LookaheadFirst.TokenType == TokenType.Wildcard)
        {
            variableSelector.VariableType = new WildcardSelectorNode();
            DiscardToken();
        }
        else if (LookaheadFirst.TokenType == TokenType.StringValue)
        {
            variableSelector.VariableType = new TypeVariableTypeSelectorNode { VariableType = LookaheadFirst.Value };
            DiscardToken();
        }
        else if (LookaheadFirst.TokenType == TokenType.OpenDoubleSquareBrackets)
        {
            DiscardToken();
            if (LookaheadFirst.TokenType == TokenType.StringValue)
            {
                variableSelector.VariableType = new MappingSelectorNode { MappingName = LookaheadFirst.Value };
                DiscardToken();

                if (LookaheadFirst.TokenType != TokenType.CloseDoubleSquareBrackets)
                {
                    // TODO - Throw error is parse was not successful
                }
                DiscardToken();
            }
            else
            {
                // TODO - Throw error is parse was not successful
            }
        }
        else
        {
            // TODO - Throw error is parse was not successful
        }

        // Step 3 - Parse variable location
        variableSelector.VariableLocation = MatchContractSelector();
        DiscardToken(TokenType.FullStop);

        // Step 4 - Parse variable name
        if (LookaheadFirst.TokenType == TokenType.Wildcard)
        {
            variableSelector.VariableNameSelector = new WildcardSelectorNode();
            DiscardToken();
        }
        else if (LookaheadFirst.TokenType == TokenType.StringValue &&
            LookaheadSecond.TokenType == TokenType.OpenSquareBrackets)
        {
            var dictionaryElementSelector = new DictionaryElementVariableNameSelectoreNode
            {
                VariableName = LookaheadFirst.Value
            };

            DiscardToken();
            DiscardToken();

            if (LookaheadFirst.TokenType == TokenType.Wildcard)
            {
                dictionaryElementSelector.KeySelector = new WildcardSelectorNode();
            }
            else if (LookaheadFirst.TokenType == TokenType.OpenSquareBrackets)
            {
                DiscardToken();
                ValidateToken(TokenType.StringValue);

                dictionaryElementSelector.KeySelector = new MappingSelectorNode { MappingName = LookaheadFirst.Value };
                DiscardToken();

                DiscardToken(TokenType.CloseDoubleSquareBrackets);
            }
            else if (LookaheadFirst.TokenType == TokenType.StringValue)
            {
                dictionaryElementSelector.KeySelector = new ConstantKeySelectorNode { Constant = LookaheadFirst.Value };

                DiscardToken();
            }

            DiscardToken(TokenType.CloseSquareBrackets);
        }
        else if (LookaheadFirst.TokenType == TokenType.StringValue &&
                 LookaheadSecond.TokenType == TokenType.FullStop)
        {
            variableSelector.VariableNameSelector = MatchPropertySelector();
        }
        else if (LookaheadFirst.TokenType == TokenType.StringValue)
        {
            DiscardToken();

            variableSelector.VariableNameSelector = new NameVariableNameSelectorNode
            {
                VariableName = LookaheadFirst.Value

            };

            DiscardToken();
        }
        else
        {
            // TODO - Throw error is parse was not successful
        }

        // Step 5 - Parse variable decorator
        if (LookaheadFirst.TokenType == TokenType.TaggedWith)
        {
            DiscardToken();
            if (LookaheadFirst.TokenType is TokenType.Public or TokenType.Private or TokenType.Internal)
            {
                variableSelector.VariableDecorator = new VariableDecoratorNode
                {
                    VariableVisibility = LookaheadFirst.GetVariableVisibility()
                };
            }
            else
            {
                // TODO - Throw error is parse was not successful
            }
        }
        else
        {
            // TODO - Throw error is parse was not successful
        }

        return variableSelector;
    }

    private NamePropertySelectorNode MatchPropertySelector()
    {
        var propertySelector = new NamePropertySelectorNode
        {
            PropertyName = LookaheadFirst.Value
        };

        DiscardToken();

        if (LookaheadFirst.TokenType == TokenType.FullStop)
        {
            DiscardToken();
            propertySelector.Child = MatchPropertySelector();
        }

        return propertySelector;
    }

    private ModificationStatementNode MatchModificationStatement()
    {
        var modificationStatement = new ModificationStatementNode();

        if (LookaheadFirst.IsModificationType())
        {
            modificationStatement.ModificationType = new ModificationTypeNode
            {
                Value = LookaheadFirst.GetModificationType()
            };
            DiscardToken();

            modificationStatement.ReferenceDefinition = MatchReferenceDefinition();

            if (LookaheadFirst.TokenType == TokenType.TaggedWith)
            {
                DiscardToken();
                modificationStatement.DefinitionDecorator = new TaggedDefinitionDecoratorNode
                {
                    ModifierSyntax = MatchModifierSyntax()
                };
            }
            else if (LookaheadFirst.TokenType == TokenType.ImplementingInterface && LookaheadSecond.TokenType == TokenType.StringValue)
            {
                DiscardToken();
                modificationStatement.DefinitionDecorator = new ImplementingDefinitionDecoratorNode
                {
                    InterfaceName = LookaheadFirst.Value
                };
            }
        }
        else
        {
            // TODO - Throw error is parse was not successful
        }

        return modificationStatement;
    }
}