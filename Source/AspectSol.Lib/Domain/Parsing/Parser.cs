using AspectSol.Lib.Domain.AST;
using AspectSol.Lib.Domain.Tokenization;
using AspectSol.Lib.Infra.Enums;
using AspectSol.Lib.Infra.Extensions;

namespace AspectSol.Lib.Domain.Parsing;

public class Parser : AbstractParser, IParser
{
    public AspectNode Parse(List<DslToken> tokens)
    {
        Initialize(tokens);

        var aspectNode = MatchAspect();

        return aspectNode;
    }

    private AspectNode MatchAspect()
    {
        DiscardToken(TokenType.Aspect, "An aspect needs to start with the 'aspect' keyword");

        ValidateToken(TokenType.ArbitraryWord, "An aspect needs to have a name associated to it");
        var aspectName = LookaheadFirst.Value;
        DiscardToken();

        DiscardToken(TokenType.OpenScope, "Aspect must be enclosed within '{}' characters. '{' character missing");

        var statements = new List<StatementNode>();
        while (LookaheadFirst.TokenType != TokenType.CloseScope)
        {
            statements.Add(MatchStatement());
        }
        
        DiscardToken(TokenType.CloseScope, "Aspect must be enclosed within '{}' characters. '}' character missing");

        return new AspectNode
        {
            Name       = aspectName,
            Statements = statements
        };
    }

    private StatementNode MatchStatement()
    {
        StatementNode node;
        if (LookaheadFirst.IsAppendStatement()) node            = MatchStatementAppend();
        else if (LookaheadFirst.IsModificationStatement()) node = MatchStatementModification();
        else throw ParserException("Currently supported statements include append statements and modification statements");

        ValidateToken(TokenType.Scope, "Any statement needs to have a scope body associated to it");
        var body = LookaheadFirst.Value.Replace("¬{", string.Empty).Replace("}¬", string.Empty);
        DiscardToken();

        node.Body = new List<ExpressionNode>
        {
            new ExpressionGenericNode
            {
                Expression = body
            }
        };

        return node;
    }

    private StatementAppendNode MatchStatementAppend()
    {
        var placement = MatchPlacement();
        var location = MatchLocation();

        var selectors = new List<SelectorNode> {MatchSelector()};

        while (LookaheadFirst.TokenType is TokenType.ReturningTypes or TokenType.InInterface or TokenType.NotInInterface)
        {
            selectors.Add(MatchSelector());
        }

        var sender = LookaheadFirst.TokenType is TokenType.OriginatingFrom
            ? MatchSender()
            : null;

        return new StatementAppendNode
        {
            Location  = location,
            Placement = placement,
            Selectors = selectors,
            Sender    = sender
        };
    }

    private SenderNode MatchSender()
    {
        DiscardToken(TokenType.OriginatingFrom, "A sender tag needs to start with the keyword 'originating-from'");

        return new SenderNode
        {
            SyntaxDefinitionNodeReference = MatchSyntaxDefinitionNodeReference()
        };
    }

    private SelectorNode MatchSelector()
    {
        if (LookaheadFirst.IsDefinitionSelector())
        {
            return MatchSelectorDefinition();
        }

        if (LookaheadFirst.TokenType is TokenType.ReturningTypes)
        {
            return MatchSelectorFunctionReturn();
        }

        if (LookaheadFirst.TokenType is TokenType.InInterface or TokenType.NotInInterface)
        {
            return MatchSelectorInterfaceFunctionName();
        }

        if (LookaheadFirst.IsVariableSelector())
        {
            return MatchSelectorVariable();
        }

        throw ParserException("A selector node can either be of type definition, returning types, interface function or variable selectors");
    }

    private SelectorVariableNode MatchSelectorVariable()
    {
        var variableAccess = MatchVariableAccessNode();

        SelectorNode variableTypeSelector;
        if (LookaheadFirst.TokenType is TokenType.Wildcard)
        {
            var wildcardFor = variableAccess.Value == VariableAccess.Get ? WildcardFor.VariableTypeGetter : WildcardFor.VariableTypeSetter;
            variableTypeSelector = MatchSelectorWildcard(wildcardFor);
        }
        else if (LookaheadFirst.TokenType is TokenType.ArbitraryWord)
        {
            variableTypeSelector = MatchSelectorVariableType(variableAccess.Value);
        }
        else if (LookaheadFirst.TokenType is TokenType.OpenDoubleSquareBrackets)
        {
            variableTypeSelector = MatchSelectorMapping();
        }
        else
        {
            throw ParserException("A variable type selector can either be of type wildcard, mapping or arbitrary word which identifies the variable type");
        }

        var variableLocation = MatchContractSelector();

        DiscardToken(TokenType.FullStop, "Variable location and variable name must be seperated by the '.' character");

        SelectorNode variableNameSelector;
        if (LookaheadFirst.TokenType is TokenType.Wildcard)
        {
            var wildcardFor = variableAccess.Value == VariableAccess.Get ? WildcardFor.VariableTypeGetter : WildcardFor.VariableTypeSetter;
            variableNameSelector = MatchSelectorWildcard(wildcardFor);
        }
        else if (LookaheadFirst.TokenType is TokenType.ArbitraryWord && LookaheadSecond.TokenType is TokenType.OpenDoubleSquareBrackets)
        {
            variableNameSelector = MatchSelectorVariableDictionaryElementName(variableAccess.Value);
        }
        else if (LookaheadFirst.TokenType is TokenType.ArbitraryWord && LookaheadSecond.TokenType is TokenType.FullStop)
        {
            variableNameSelector = MatchSelectorVariablePropertyName();
        }
        else if (LookaheadFirst.TokenType is TokenType.ArbitraryWord)
        {
            variableNameSelector = MatchSelectorVariableName(variableAccess.Value);
        }
        else
        {
            throw ParserException("A variable name selector can either be of type wildcard, mapping, property or arbitrary word which identifies " +
                "the variable type");
        }

        var decoratorVariable = MatchDecoratorVariable(variableAccess.Value);

        return new SelectorVariableNode
        {
            VariableAccessNode   = variableAccess,
            VariableType         = variableTypeSelector,
            VariableLocation     = variableLocation,
            VariableNameSelector = variableNameSelector,
            DecoratorVariable    = decoratorVariable
        };
    }

    private DecoratorVariableNode MatchDecoratorVariable(VariableAccess variableAccess)
    {
        DiscardToken(TokenType.TaggedWith, "Variable decorator tag must start with the 'tagged-with' keyword");

        VariableVisibility variableVisibility;
        if (LookaheadFirst.TokenType == TokenType.Public) variableVisibility        = VariableVisibility.Public;
        else if (LookaheadFirst.TokenType == TokenType.Private) variableVisibility  = VariableVisibility.Private;
        else if (LookaheadFirst.TokenType == TokenType.Internal) variableVisibility = VariableVisibility.Internal;
        else throw ParserException(@"Supported variable visibilities include 'public', 'private' and 'internal'");

        DiscardToken();

        return new DecoratorVariableNode(variableAccess)
        {
            VariableVisibility = variableVisibility
        };
    }

    private SelectorVariableNameNode MatchSelectorVariableName(VariableAccess variableAccess)
    {
        ValidateToken(TokenType.ArbitraryWord, "Selector variable name must start with a variable name");
        var variableName = LookaheadFirst.Value;
        DiscardToken();

        return new SelectorVariableNameNode(variableAccess)
        {
            VariableName = variableName
        };
    }

    private SelectorVariablePropertyNameNode MatchSelectorVariablePropertyName()
    {
        ValidateToken(TokenType.ArbitraryWord, "Variable property name selector must be an arbitrary word");
        var propertyName = LookaheadFirst.Value;
        DiscardToken();

        SelectorVariablePropertyNameNode child = null;
        if (LookaheadFirst.TokenType == TokenType.FullStop)
        {
            DiscardToken();
            child = MatchSelectorVariablePropertyName();
        }

        return new SelectorVariablePropertyNameNode
        {
            PropertyName = propertyName,
            Child        = child
        };
    }

    private SelectorVariableDictionaryElementNameNode MatchSelectorVariableDictionaryElementName(VariableAccess variableAccess)
    {
        ValidateToken(TokenType.ArbitraryWord, "Variable dictionary element name selector must be an arbitrary word");
        var variableName = LookaheadFirst.Value;
        DiscardToken();

        SelectorNode keySelector;
        if (LookaheadFirst.TokenType is TokenType.Wildcard)
        {
            keySelector = MatchSelectorWildcard(WildcardFor.DictionaryElement);
        }
        else if (LookaheadFirst.TokenType == TokenType.OpenSquareBrackets)
        {
            keySelector = MatchSelectorMapping();
        }
        else if (LookaheadFirst.TokenType == TokenType.StringValue)
        {
            keySelector = MatchSelectorVariableConstantKey();
        }
        else
        {
            throw ParserException("Key selector for dictionary element selector can either be a wildcard, mapping or constant selector");
        }

        return new SelectorVariableDictionaryElementNameNode(variableAccess)
        {
            VariableName = variableName,
            KeySelector  = keySelector
        };
    }

    private SelectorVariableConstantKeyNode MatchSelectorVariableConstantKey()
    {
        ValidateToken(TokenType.ArbitraryWord, "Variable constant selector needs to be an arbitrary word");
        var constant = LookaheadFirst.Value;
        DiscardToken();

        return new SelectorVariableConstantKeyNode
        {
            Constant = constant
        };
    }

    private SelectorVariableTypeNode MatchSelectorVariableType(VariableAccess variableAccess)
    {
        ValidateToken(TokenType.ArbitraryWord, "A selector variable type must start with an arbitrary word denoting the variable type");
        var variableType = LookaheadFirst.Value;
        DiscardToken();

        return new SelectorVariableTypeNode(variableAccess, variableType);
    }

    private VariableAccessNode MatchVariableAccessNode()
    {
        VariableAccess variableAccess;
        if (LookaheadFirst.TokenType == TokenType.Get) variableAccess      = VariableAccess.Get;
        else if (LookaheadFirst.TokenType == TokenType.Set) variableAccess = VariableAccess.Set;
        else throw ParserException(@"Supported variable access tags tags include 'in-interface' and 'not-in-interface'");

        DiscardToken();
        return new VariableAccessNode(variableAccess);
    }

    private SelectorInterfaceFunctionNameNode MatchSelectorInterfaceFunctionName()
    {
        var interfaceTag = MatchInterfaceTag();

        ValidateToken(TokenType.ArbitraryWord, "Selector interface function name must be an arbitrary word");
        var interfaceName = LookaheadFirst.Value;
        DiscardToken();

        return new SelectorInterfaceFunctionNameNode
        {
            InterfaceName    = interfaceName,
            InterfaceTagNode = interfaceTag
        };
    }

    private InterfaceTagNode MatchInterfaceTag()
    {
        InterfaceTag interfaceTag;
        if (LookaheadFirst.TokenType == TokenType.InInterface) interfaceTag         = InterfaceTag.InInterface;
        else if (LookaheadFirst.TokenType == TokenType.NotInInterface) interfaceTag = InterfaceTag.NotInInterface;
        else throw ParserException(@"Supported interface tags include 'in-interface' and 'not-in-interface'");

        DiscardToken();
        return new InterfaceTagNode(interfaceTag);
    }

    private SelectorFunctionReturnNode MatchSelectorFunctionReturn()
    {
        DiscardToken(TokenType.ReturningTypes, "Selector function returns must start with the keyword 'returning-types'");

        DiscardToken(TokenType.OpenParenthesis, "Selector function returns must be enclosed within '()' characters. '(' character missing");

        var returns = new List<string>();
        while(LookaheadFirst.TokenType is TokenType.ArbitraryWord)
        {
            returns.Add(LookaheadFirst.Value);

            if (LookaheadFirst.TokenType is not TokenType.CloseParenthesis && LookaheadFirst.TokenType is not TokenType.Comma)
            {
                throw ParserException("A comma must be inserted between return definitions");
            }
        }

        DiscardToken(TokenType.CloseParenthesis, "Selector function returns must be enclosed within '()' characters. ')' character missing");

        return new SelectorFunctionReturnNode
        {
            Returns = returns,
        };
    }

    private SelectorDefinitionNode MatchSelectorDefinition()
    {
        var syntaxDefinition = MatchSyntaxDefinitionNodeReference();
        
        DecoratorDefinitionNode decoratorDefinition = null;
        if (LookaheadFirst.TokenType is TokenType.TaggedWith or TokenType.ImplementingInterface)
        {
            decoratorDefinition = MatchDecorator();
        }
        
        return new SelectorDefinitionNode
        {
            SyntaxDefinition    = syntaxDefinition,
            DecoratorDefinition = decoratorDefinition
        };
    }

    private LocationNode MatchLocation()
    {
        Location location;
        if (LookaheadFirst.TokenType == TokenType.CallTo) location           = Location.CallTo;
        else if (LookaheadFirst.TokenType == TokenType.ExecutionOf) location = Location.ExecutionOf;
        else throw ParserException(@"Supported locations include 'call-to' and 'execution-of'");

        DiscardToken();
        return new LocationNode(location);
    }

    private PlacementNode MatchPlacement()
    {
        Placement placement;
        if (LookaheadFirst.TokenType == TokenType.Before) placement     = Placement.Before;
        else if (LookaheadFirst.TokenType == TokenType.After) placement = Placement.After;
        else throw ParserException(@"Supported placements include 'before' and 'after'");

        DiscardToken();
        return new PlacementNode(placement);
    }

    private StatementModificationNode MatchStatementModification()
    {
        var modificationType = MatchModificationType();
        var syntaxDefinitionReference = MatchSyntaxDefinitionNodeReference();

        DecoratorDefinitionNode decoratorDefinition = null;
        if (LookaheadFirst.TokenType is TokenType.TaggedWith or TokenType.ImplementingInterface)
        {
            decoratorDefinition = MatchDecorator();
        }
        
        return new StatementModificationNode
        {
            ModificationType                  = modificationType,
            SyntaxDefinitionNodeReferenceNode = syntaxDefinitionReference,
            DecoratorDefinition               = decoratorDefinition
        };
    }

    private DecoratorDefinitionNode MatchDecorator()
    {
        if (LookaheadFirst.TokenType is TokenType.TaggedWith)
        {
            return MatchDecoratorDefinitionTagged();
        }

        if (LookaheadFirst.TokenType is TokenType.ImplementingInterface)
        {
            return MatchDecoratorDefinitionImplementing();
        }

        throw ParserException("A decorator can either be of type 'tagged-with' or 'implementing-interface'");
    }

    private DecoratorDefinitionImplementingNode MatchDecoratorDefinitionImplementing()
    {
        DiscardToken(TokenType.TaggedWith, "An implementing interface definition decorator must start with the 'implementing-interface' keyword");

        ValidateToken(TokenType.ArbitraryWord, "The 'implementing-interface' keyword must be followed by an interface name");
        var interfaceName = LookaheadFirst.Value;
        DiscardToken();

        return new DecoratorDefinitionImplementingNode
        {
            InterfaceName = interfaceName
        };
    }

    private DecoratorDefinitionTaggedNode MatchDecoratorDefinitionTagged()
    {
        DiscardToken(TokenType.TaggedWith, "A tagged with definition decorator must start with the 'tagged-with' keyword");

        return new DecoratorDefinitionTaggedNode
        {
            SyntaxModifier = MatchSyntaxModifier()
        };
    }

    private SyntaxModifierNode MatchSyntaxModifier()
    {
        if (LookaheadFirst.TokenType == TokenType.OpenParenthesis)
        {
            DiscardToken();

            return new SyntaxModifierNode
            {
                Left = MatchSyntaxModifier()
            };
        }

        if (LookaheadFirst.TokenType == TokenType.NotSymbol)
        {
            DiscardToken();

            return new SyntaxModifierNode
            {
                Operator = ModifierOperator.Not,
                Left     = MatchSyntaxModifier()
            };
        }

        if (LookaheadFirst.IsModifierOrVisibility())
        {
            var modifierSyntax = new SyntaxModifierNode
            {
                Left = new ModifierNode {ModifierName = LookaheadFirst.Value}
            };
            DiscardToken();

            if (LookaheadFirst.TokenType is not (TokenType.OrSymbol or TokenType.AndSymbol))
            {
                return modifierSyntax;
            }

            modifierSyntax.Operator = LookaheadFirst.TokenType == TokenType.AndSymbol
                ? ModifierOperator.And
                : ModifierOperator.Or;
            DiscardToken();

            modifierSyntax.Right = MatchSyntaxModifier();
        }

        throw ParserException("Failed to parse tokens to formulate a valid syntax modifier condition");
    }

    private SyntaxDefinitionNodeReference MatchSyntaxDefinitionNodeReference()
    {
        var contractSelector = MatchContractSelector();

        SelectorNode functionSelector = null;
        if (LookaheadFirst.TokenType is TokenType.FullStop)
        {
            DiscardToken(TokenType.FullStop, "Contract selector and function selector must be seperated by the '.' character");

            functionSelector = MatchFunctionSelector();
        }

        return new SyntaxDefinitionNodeReference
        {
            ContractSelector = contractSelector,
            FunctionSelector = functionSelector
        };
    }

    private SelectorNode MatchFunctionSelector()
    {
        if (LookaheadFirst.TokenType is TokenType.Wildcard)
        {
            return MatchSelectorWildcard(WildcardFor.Function);
        }

        if (LookaheadFirst.TokenType is TokenType.ArbitraryWord)
        {
            return MatchSelectorFunctionName();
        }

        throw ParserException("Function selector can either be of type wildcard or function name");
    }

    private SelectorFunctionNameNode MatchSelectorFunctionName()
    {
        if (LookaheadSecond.TokenType is TokenType.OpenParenthesis)
        {
            return MatchSelectorFunctionParameters();
        }

        ValidateToken(TokenType.ArbitraryWord, "Selector function name must start with a function name");
        var functionName = LookaheadFirst.Value;
        DiscardToken();

        return new SelectorFunctionNameNode
        {
            FunctionName = functionName
        };
    }

    private SelectorFunctionParametersNode MatchSelectorFunctionParameters()
    {
        ValidateToken(TokenType.ArbitraryWord, "Selector function parameters must start with a function name");
        var functionName = LookaheadFirst.Value;
        DiscardToken();

        DiscardToken(TokenType.OpenParenthesis, "Selector function parameters must be enclosed within '()' characters. '(' character missing");

        var parameters = new List<ParameterNode>();
        while(LookaheadFirst.TokenType is TokenType.ArbitraryWord)
        {
            var parameter = MatchParameterNode();
            parameters.Add(parameter);

            if (LookaheadFirst.TokenType is not TokenType.CloseParenthesis && LookaheadFirst.TokenType is not TokenType.Comma)
            {
                throw ParserException("A comma must be inserted between parameter definitions");
            }
        }

        DiscardToken(TokenType.CloseParenthesis, "Selector function parameters must be enclosed within '()' characters. ')' character missing");

        return new SelectorFunctionParametersNode
        {
            FunctionName = functionName,
            Parameters   = parameters
        };
    }

    private ParameterNode MatchParameterNode()
    {
        ValidateToken(TokenType.ArbitraryWord, "A parameter node must start with a type");
        var type = LookaheadFirst.Value;
        DiscardToken();

        ValidateToken(TokenType.ArbitraryWord, "The type within a parameter node must be preceded by a name");
        var name = LookaheadFirst.Value;
        DiscardToken();

        return new ParameterNode
        {
            Type = type,
            Name = name
        };
    }

    private SelectorNode MatchContractSelector()
    {
        if (LookaheadFirst.TokenType is TokenType.Wildcard)
        {
            return MatchSelectorWildcard(WildcardFor.Contract);
        }

        if (LookaheadFirst.TokenType is TokenType.OpenDoubleSquareBrackets)
        {
            return MatchSelectorMapping();
        }

        if (LookaheadFirst.TokenType is TokenType.Address)
        {
            return MatchSelectorContractAddress();
        }

        if (LookaheadFirst.TokenType == TokenType.ArbitraryWord && LookaheadSecond.TokenType == TokenType.DoubleColon)
        {
            return MatchSelectorInterfaceContract();
        }

        if (LookaheadFirst.TokenType == TokenType.ArbitraryWord)
        {
            return MatchSelectorContractName();
        }

        throw ParserException("Contract selector can either be of type wildcard, mapping, address, interface or contract name");
    }

    private SelectorWildcardNode MatchSelectorWildcard(WildcardFor wildcardFor)
    {
        DiscardToken(TokenType.Wildcard, "A wildcard selector must start with the '*' symbol");

        return new SelectorWildcardNode
        {
            WildcardFor = wildcardFor
        };
    }

    private SelectorMappingNode MatchSelectorMapping()
    {
        DiscardToken(TokenType.OpenDoubleSquareBrackets, "Selector mapping must be enclosed within '[[]]' characters. '[[' character missing");

        ValidateToken(TokenType.ArbitraryWord, "Mapping name missing for selector mapping");
        var mappingName = LookaheadFirst.Value;
        DiscardToken();

        DiscardToken(TokenType.CloseDoubleSquareBrackets, "Selector mapping must be enclosed within '[[]]' characters. ']]' character missing");

        return new SelectorMappingNode
        {
            MappingName = mappingName
        };
    }

    private SelectorContractAddressNode MatchSelectorContractAddress()
    {
        ValidateToken(TokenType.Address, "Selector contract address must be an Ethereum address which starts with the characters '0x' and " +
            "after contains 40 hexadecimal characters");
        var contractAddress = LookaheadFirst.Value;
        DiscardToken();

        return new SelectorContractAddressNode
        {
            ContractAddress = contractAddress
        };
    }

    private SelectorInterfaceContractNode MatchSelectorInterfaceContract()
    {
        ValidateToken(TokenType.ArbitraryWord, "Selector interface contract must start with a contract name");
        var contractName = LookaheadFirst.Value;
        DiscardToken();

        ValidateToken(TokenType.DoubleColon, "Contract name of a selector interface contract must be proceeded with the '::' character");

        ValidateToken(TokenType.ArbitraryWord, "Selector interface contract must end with an interface name");
        var interfaceName = LookaheadFirst.Value;
        DiscardToken();

        return new SelectorInterfaceContractNode
        {
            ContractName  = contractName,
            InterfaceName = interfaceName
        };
    }

    private SelectorContractNameNode MatchSelectorContractName()
    {
        ValidateToken(TokenType.ArbitraryWord, "Selector contract name must start with a contract name");
        var contractName = LookaheadFirst.Value;
        DiscardToken();

        return new SelectorContractNameNode
        {
            ContractName = contractName
        };
    }

    private ModificationTypeNode MatchModificationType()
    {
        ModificationType modificationType;
        if (LookaheadFirst.TokenType == TokenType.AddToDeclaration) modificationType      = ModificationType.AddToDeceleration;
        else if (LookaheadFirst.TokenType == TokenType.UpdateDefinition) modificationType = ModificationType.UpdateDefinition;
        else throw ParserException(@"Supported modification types are include 'add-to-declaration' and 'update-definition'");

        DiscardToken();
        return new ModificationTypeNode(modificationType);
    }
}