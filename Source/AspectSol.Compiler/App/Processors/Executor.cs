using AspectSol.Compiler.App.SolidityProcessors;
using AspectSol.Compiler.Domain;
using AspectSol.Compiler.Domain.AST;
using AspectSol.Compiler.Infra.Models;
using AspectSol.Compiler.Infra.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using AspectSol.Compiler.Infra.Helpers;
using AspectSol.Compiler.Infra.Exceptions;

namespace AspectSol.Compiler.App.Processors
{
    public class Executor
    {
        private readonly SolidityTransformer _transformer;
        private readonly List<LocalVariable> _locals;

        public Executor(SolidityTransformer transformer)
        {
            _transformer = transformer ?? throw new ArgumentNullException(nameof(IContractTransformer));
            _locals = new List<LocalVariable>();
        }

        /// <summary>
        /// Execute all aspect statements on the passed container
        /// </summary>
        /// <param name="statements"></param>
        /// <returns></returns>
        public void Start(List<StatementNode> statements, JContainer container)
        {
            foreach(var statement in statements)
            {
                _ = Execute(statement, container);
            }
        }

        /// <summary>
        /// Execute aspect statement on the passed container
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public SelectionResult Execute(StatementNode node, JContainer container)
        {
            SelectionResult selection;
            if (node.GetType() == typeof(AppendStatementNode))
            {
                selection = Execute(node as AppendStatementNode, container);
            }
            else if (node.GetType() == typeof(ModificationStatementNode))
            {
                selection = Execute(node as ModificationStatementNode, container);
            }
            else
            {
                throw new InvalidStatementNodeException(
                    $"Invalid statement node type encountered: {node.GetType()}");
            }

            return selection;
        }

        /// <summary>
        ///  Filter the container to find variable definitions defined using ContractAddress passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(AddressContractSelectorNode node, JContainer container)
        {
            // TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// Execute the append statement using parameters passed in node on the passed container
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public SelectionResult Execute(AppendStatementNode node, JContainer container)
        {
            var selection = new SelectionResult { Container = container };

            foreach (var selector in node.Selectors)
            {
                selection = Execute(selector, container);
            }

            switch (node.Placement.Value)
            {
                case Placement.BEFORE:
                {
                    var modifierName = StringExtensions.Instance.GenerateRandomString(8);

                    var newModifier = _transformer.GenerateModifierNode(modifierName);
                    //_transformer.AddContractSubNode(selection.Container, newModifier);

                    var newFunctionModifier = _transformer.GenerateFunctionModifierNode(modifierName);
                    _transformer.AddFunctionModifier(selection.Container, newFunctionModifier);
                    break;
                }
                case Placement.AFTER when node.Location.Value == Location.CALLTO:
                {
                    // TODO - IN THIS CASE WE NEED TO ADD A STATEMENT AFTER THE STATEMENT OF THE FUNCTION CALL
                    //var newStatements = _transformer.GenerateFunctionStatements(node.Body);
                    break;
                }
                case Placement.AFTER when node.Location.Value == Location.EXECUTIONOF:
                {
                    // TODO - IN THIS CASE WE NEED TO ADD A STATEMENT AT THE END OF THE LIST OF THE FUNCTION DEFINITION
                    //var newStatements = _transformer.GenerateFunctionStatements(node.Body);
                    //_transformer.AddFunctionStatements(selection.Container, newStatements);
                    break;
                }
                case Placement.AFTER:
                    throw new InvalidLocationNodeException(
                        $"Invalid location node encountered while executing AppendStatementNode: {node.Location.Value}");
                default:
                    throw new InvalidPlacementNodeException(
                        $"Invalid placement node encountered while executing AppendStatementNode: {node.Placement.Value}");
            }

            return selection;
        }

        /// <summary>
        /// Filter the container to find statements using the Constant passed in the node as an access key
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public SelectionResult Execute(ConstantKeySelectorNode node, JContainer container)
        {
            var selection = _transformer.FilterVariableGettersByAccessKey(container, node.Constant);

            return selection;
        }

        /// <summary>
        /// Filter the container using the parameters passed in the selector node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(DefinitionSelectorNode node, JContainer container)
        {
            var selection = new SelectionResult { Container = container };

            if(node.DefinitionSyntax.GetType() == typeof(InstanceDefinitionSyntaxNode))
            {
                // TODO - Execute InstanceDefinitionSyntaxNode
            }
            else if(node.DefinitionSyntax.GetType() == typeof(ReferenceDefinitionSyntaxNode))
            {
                var referenceDefinition = node.DefinitionSyntax as ReferenceDefinitionSyntaxNode;

                selection = Execute(referenceDefinition?.ContractSelector, container);
                selection = Execute(referenceDefinition?.FunctionSelector, selection.Container);
            }
            else
            {
                throw new InvalidDefinitionSyntaxNodeException(
                    $"Invalid definition syntax node type encountered while executing DefinitionSelectorNode: {node.DefinitionSyntax.GetType()}");
            }

            return selection;
        }

        /// <summary>
        /// Filter the container to find statements using VariableName with a KeySelector constant as an access key
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(DictionaryElementVariableNameSelectoreNode node, JContainer container)
        {
            SelectionResult selection;
            if(node.KeySelector.GetType() == typeof(ConstantKeySelectorNode))
            {
                var keySelector = node.KeySelector as ConstantKeySelectorNode;

                selection = _transformer.FilterVariableGettersByVariableName(container, node.VariableName);
                selection = Execute(keySelector, selection.Container);
            }
            else
            {
                throw new InvalidSelectorNodeException(
                    $"Invalid selector node type encountered while executing DictionaryElementVariableNameSelectorNode: {node.KeySelector.GetType()}");
            }

            return selection;
        }

        /// <summary>
        /// Filter the container using the parameters passed defined in the instance definition syntax node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(InstanceDefinitionSyntaxNode node, JContainer container)
        {
            string functionName;
            if (node.FunctionSelector.GetType() == typeof(NameFunctionSelectorNode))
            {
                functionName = (node.FunctionSelector as NameFunctionSelectorNode)?.FunctionName;
            }
            else
            {
                throw new InvalidFunctionSelectorNodeException(
                    $"Invalid function selector node type encountered while executing InstanceDefinitionSyntaxNode: {node.FunctionSelector.GetType()}");
            }

            string instanceName;
            if (node.InstanceSelector.GetType() == typeof(NameInstanceSelectorNode))
            {
                instanceName = (node.InstanceSelector as NameInstanceSelectorNode)?.InstanceName;
            }
            else
            {
                throw new InvalidInstanceSelectorNodeException(
                    $"Invalid instance selector node type encountered while executing InstanceDefinitionSyntaxNode: {node.InstanceSelector.GetType()}");
            }

            return _transformer.FilterFunctionCallsByInstanceName(container, functionName, instanceName);
        }

        /// <summary>
        /// Filter the container to find contracts whose name matches ContractName and implements InterfaceName passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(InterfaceContractSelectorNode node, JContainer container)
        {
            var selection = _transformer.FilterContractsByContractName(container, node.ContractName);
            selection = _transformer.FilterContractsByInterfaceName(selection.Container, node.InterfaceName);

            return selection;
        }

        /// <summary>
        /// Filter the container with items implementing items from interface matching InterfaceName passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public SelectionResult Execute(InterfaceSyntaxNode node, JContainer container)
        {
            if(node.InterfaceSelector.GetType() == typeof(NameInterfaceSelectorNode))
            {
                var interfaceSelector = node.InterfaceSelector as NameInterfaceSelectorNode;
                var invert = node.InterfaceNode.Value == Interface.NOTININTERFACE;
                var selection = _transformer.FilterContainerByInterfaceImplementation(container, interfaceSelector?.InterfaceName, invert);

                return selection;
            }
            else
            {
                return new SelectionResult
                {
                    Container = container
                };
            }
        }

        /// <summary>
        /// Filter the container using the value found in the MappingName variable passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(MappingSelectorNode node, JContainer container)
        {
            var variableValue = _locals.Where(local => local.Name.Equals(node.MappingName)).FirstOrDefault();
            
            if(variableValue != null)
            {
                var selection = _transformer.FilterContractsByContractName(container, variableValue.Name);

                return selection;
            }
            else
            {
                throw new InvalidMappingException($"Mapping '{node.MappingName}' has not been declared");
            }
        }

        /// <summary>
        /// Modify the definition or body of the matched results from ReferenceDefinition passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(ModificationStatementNode node, JContainer container)
        {
            switch (node.ModificationType.Value)
            {
                case ModificationType.ADDTODECLARATION:
                    // TODO - Take contents of aspect and put in matched selection
                    break;
                case ModificationType.UPDATEDEFINITION:
                    break;
                default:
                    // TODO - Throw an error
                    break;
            }

            return new SelectionResult
            {
                Container = container
            };
        }

        /// <summary>
        /// Filter the container to find contracts containing functions who match the ModifierName passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public SelectionResult Execute(ModifierNode node, JContainer container)
        {
            var selection = _transformer.FilterFunctionsByModifier(container, node.ModifierName, false);

            return selection;
        }

        /// <summary>
        /// Filter the container to find contracts containing functions who match the modifiers passed in the node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public SelectionResult Execute(ModifierSyntaxNode node, JContainer container)
        {
            var selection = node.Operator switch
            {
                ModifierOperator.NOT => _transformer.FilterFunctionsByModifier(container, node.Left.ModifierName, true),
                ModifierOperator.AND => _transformer.FilterFunctionsByAllModifiers(container,
                    new List<string>() { node.Left.ModifierName, node.Right.ModifierName }),
                ModifierOperator.OR => _transformer.FilterFunctionsByEitherModifiers(container,
                    new List<string>() { node.Left.ModifierName, node.Right.ModifierName }),
                ModifierOperator.NONE => _transformer.FilterFunctionsByModifier(container, node.Left.ModifierName,
                    false),
                _ => new SelectionResult { Container = container }
            };

            return selection;
        }

        /// <summary>
        /// Filter the container to find contracts whose name matches ContractName passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(NameContractSelectorNode node, JContainer container)
        {
            var selection = _transformer.FilterContractsByContractName(container, node.ContractName);

            return selection;
        }

        /// <summary>
        /// Filter the container to find contracts containing functions whose name matches FunctionName passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(NameFunctionSelectorNode node, JContainer container)
        {
            var selection = _transformer.FilterFunctionsByFunctionName(container, node.FunctionName);

            return selection;
        }

        /// <summary>
        /// Filter the container with items implementing items from interface matching InterfaceName passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(NameInterfaceSelectorNode node, JContainer container)
        {
            var selection = _transformer.FilterContainerByInterfaceImplementation(container, node.InterfaceName, false);

            return selection;
        }

        public SelectionResult Execute(NamePropertySelectorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Filter the container to find contracts containing variables having name matching VariableName in passed node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(NameVariableNameSelectorNode node, JContainer container)
        {
            var selection = _transformer.FilterVariableDefinitionByVariableName(container, node.VariableName);

            return selection;
        }

        /// <summary>
        /// Filter the container to find contracts containing functions who have parameter matching Type and Name found in passed node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public SelectionResult Execute(ParameterNode node, JContainer container)
        {
            var selection = _transformer.FilterFunctionsByParameters(container, node.Type, node.Name);

            return selection;
        }

        /// <summary>
        /// Filter the container to find contracts matching parameters passed in ContractSelector and FuncitonSelector in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public SelectionResult Execute(ReferenceDefinitionSyntaxNode node, JContainer container)
        {
            var selection = Execute(node.ContractSelector, container);
            selection = Execute(node.FunctionSelector, selection.Container);

            return selection;
        }

        /// <summary>
        /// Filter the container with functions having return types matching ReturnTypes passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(ReturnSelectorNode node, JContainer container)
        {
            var selection = new SelectionResult { Container = container };

            return node.ReturnTypes.Aggregate(selection, 
                (current, returnNode) => Execute(returnNode, current.Container));
        }

        /// <summary>
        /// Filter the container with functions having return type matching Type and Name passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(ReturnTypeNode node, JContainer container)
        {
            var selection = _transformer.FilterFunctionsByReturnParameters(container, node.Type);

            return selection;
        }

        /// <summary>
        /// Filter the container the find calls originating from a location matching parameters passed in ReferenceDefinitionSyntaxNode in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public SelectionResult Execute(SenderNode node, JContainer container)
        {
            var selection = Execute(node.ReferenceDefinition, container);

            return selection;
        }

        /// <summary>
        /// Filter the container with functions having modifiers matching conditional ModifierSyntax passed in node 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public SelectionResult Execute(TaggedDefinitionDecoratorNode node, JContainer container)
        {
            var selection = Execute(node.ModifierSyntax, container);

            return selection;
        }

        /// <summary>
        /// Filter the container to find statements using variables with a type matching VariableType passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public SelectionResult Execute(TypeVariableTypeSelectorNode node, JContainer container)
        {
            var selection = _transformer.FilterVariableGettersByVariableType(container, node.VariableType);

            return selection;
        }

        /// <summary>
        /// Filter the container to find statements using variables with a visibility matching VariableVisibility passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(VariableDecoratorNode node, JContainer container)
        {
            var selection = _transformer.FilterVariableGettersByVisibility(container, node.VariableVisibility.ToString().ToLower());

            return selection;
        }

        /// <summary>
        /// Execute the variable selector statement using parameters passed in node on the passed container
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public SelectionResult Execute(VariableSelectorNode node, JContainer container)
        {
            var selection = new SelectionResult{ Container = container };
            if (node.VariableSyntaxNode.Value == VariableAccess.GET)
            {
                if(node.VariableType.GetType() == typeof(TypeVariableTypeSelectorNode))
                {
                    selection = Execute(node.VariableType as TypeVariableTypeSelectorNode, selection.Container);
                }
                else
                {
                    throw new InvalidVariableTypeSelectorNodeException(
                        $"Invalid variable type selector node type encountered while executing VariableSelectorNode: " +
                        $"{node.VariableType}");
                }

                selection = Execute(node.VariableLocation, selection.Container);

                if(node.VariableNameSelector.GetType() == typeof(NameVariableNameSelectorNode))
                {
                    selection = Execute(node.VariableType as NameVariableNameSelectorNode, selection.Container);
                }
                else if(node.VariableNameSelector.GetType() == typeof(DictionaryElementVariableNameSelectoreNode))
                {
                    selection = Execute(node.VariableType as DictionaryElementVariableNameSelectoreNode, selection.Container);
                }
                else
                {
                    throw new InvalidVariableNameSelectorNodeException(
                        $"Invalid variable name selector node type encountered while executing VariableSelectorNode: " +
                        $"{node.VariableType}");
                }

                selection = Execute(node.VariableDecorator, selection.Container);
            }
            else if (node.VariableSyntaxNode.Value == VariableAccess.SET)
            {
                if (node.VariableType.GetType() == typeof(TypeVariableTypeSelectorNode))
                {
                    selection = Execute(node.VariableType as TypeVariableTypeSelectorNode, selection.Container);
                }
                else
                {
                    throw new InvalidVariableTypeSelectorNodeException(
                        $"Invalid variable type selector node type encountered while executing VariableSelectorNode: " +
                        $"{node.VariableType}");
                }

                selection = Execute(node.VariableLocation, selection.Container);

                if (node.VariableNameSelector.GetType() == typeof(NameVariableNameSelectorNode))
                {
                    selection = Execute(node.VariableType as NameVariableNameSelectorNode, selection.Container);
                }
                else if (node.VariableNameSelector.GetType() == typeof(DictionaryElementVariableNameSelectoreNode))
                {
                    selection = Execute(node.VariableType as DictionaryElementVariableNameSelectoreNode, selection.Container);
                }
                else
                {
                    throw new InvalidVariableNameSelectorNodeException(
                        $"Invalid variable name selector node type encountered while executing VariableSelectorNode: " +
                        $"{node.VariableType}");
                }

                selection = Execute(node.VariableDecorator, selection.Container);
            }
            else
            {
                throw new InvalidVariableAccessNodeException(
                    $"Invalid variable access node type encountered while executing VariableSelectorNode: " +
                    $"{node.VariableSyntaxNode.Value}");
            }

            return selection;
        }

        public SelectionResult Execute(SelectorNode node, JContainer container)
        {
            var nodeType = node.GetType();

            if (nodeType == typeof(AddressContractSelectorNode))
            {
                return Execute(node as AddressContractSelectorNode, container);
            }
            else if (nodeType == typeof(DefinitionSelectorNode))
            {
                return Execute(node as DefinitionSelectorNode, container);
            }
            else if (nodeType == typeof(MappingSelectorNode))
            {
                return Execute(node as MappingSelectorNode, container);
            }
            else if (nodeType == typeof(PropertySelectorNode))
            {
                return Execute(node as PropertySelectorNode, container);
            }
            else if (nodeType == typeof(ReturnSelectorNode))
            {
                return Execute(node as ReturnSelectorNode, container);
            }
            else if (nodeType == typeof(VariableSelectorNode))
            {
                return Execute(node as VariableSelectorNode, container);
            }
            else if (nodeType == typeof(NameInstanceSelectorNode))
            {
                return Execute(node as NameInstanceSelectorNode, container);
            }
            else if (nodeType == typeof(InterfaceContractSelectorNode))
            {
                return Execute(node as InterfaceContractSelectorNode, container);
            }
            else if (nodeType == typeof(NameContractSelectorNode))
            {
                return Execute(node as NameContractSelectorNode, container);
            }
            else
            {
                throw new InvalidFunctionSelectorNodeException(
                    $"Invalid selector node type encountered: {node.GetType()}");
            }
        }

        public void Execute(AspectExpressionNode node, JContainer container)
        {
            if(node.GetType() == typeof(AspectAddTagExpressionNode))
            {
                Execute(node as AspectAddTagExpressionNode, container);
            }
            else if(node.GetType() == typeof(AspectGenericExpressionNode))
            {
                Execute(node as AspectGenericExpressionNode, container);
            }
            else if(node.GetType() == typeof(AspectImplementExpressionNode))
            {
                Execute(node as AspectImplementExpressionNode, container);
            }
            else if(node.GetType() == typeof(AspectRemoveTagExpressionNode))
            {
                Execute(node as AspectRemoveTagExpressionNode, container);
            }
            else if(node.GetType() == typeof(AspectUpdateTagExpressionNode))
            {
                Execute(node as AspectUpdateTagExpressionNode, container);
            }
            else
            {
                throw new InvalidStatementNodeException(
                    $"Invalid aspect expression node type encountered: {node.GetType()}");
            }
        }

        public void Execute(AspectAddTagExpressionNode node, JContainer container, Dictionary<string, string> interestedFunctions)
        {
            _transformer.AddTagToFunctions(container, interestedFunctions, node.Modifier);
        }

        public void Execute(AspectGenericExpressionNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(AspectImplementExpressionNode node, JContainer container, List<string> interestedContracts)
        {
            
        }

        public void Execute(AspectRemoveTagExpressionNode node, JContainer container, Dictionary<string, string> interestedFunctions)
        {
            _transformer.RemoveTagFromFunctions(container, interestedFunctions, node.Modifier);
        }

        public void Execute(AspectUpdateTagExpressionNode node, JContainer container)
        {
            throw new NotImplementedException();
        }
    }
}