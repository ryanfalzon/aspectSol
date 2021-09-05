using AspectSol.Compiler.App.SolidityProcessors;
using AspectSol.Compiler.Domain;
using AspectSol.Compiler.Domain.AST;
using AspectSol.Compiler.Infra.Models;
using AspectSol.Compiler.Infra.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AspectSol.Compiler.App.Processors
{
    public class Executor
    {
        private readonly SolidityTransformer _transformer;

        public Executor(SolidityTransformer transformer)
        {
            _transformer = transformer ?? throw new ArgumentNullException(nameof(IContractTransformer));
        }

        public void Execute(AddressContractSelectorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(AppendStatementNode node, JContainer container)
        {
            throw new NotImplementedException();
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

        public void Execute(DefinitionSelectorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Filter the container to find statements using VariableName with a KeySelector constant as an access key
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(DictionaryElementVariableNameSelectoreNode node, JContainer container)
        {
            if(node.KeySelector.GetType() == typeof(ConstantKeySelectorNode))
            {
                var keySelector = node.KeySelector as ConstantKeySelectorNode;

                var selection = _transformer.FilterVariableGettersByVariableName(container, node.VariableName);
                selection = Execute(keySelector, selection.Container);

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

        public void Execute(ImplementingDefinitionDecoratorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(InstanceDefinitionSyntaxNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Filter the container to find contracts whose name matches ContractName and implements InterfaceName passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(InterfaceContractSelectorNode node, JContainer container)
        {
            var selction = _transformer.FilterContractsByContractName(container, node.ContractName);
            selction = _transformer.FilterContractsByInterfaceName(selction.Container, node.InterfaceName);

            return selction;
        }

        public void Execute(InterfaceNode node, JContainer container)
        {
            throw new NotImplementedException();
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
                bool invert = node.InterfaceNode.Interface == Interface.NOTININTERFACE;
                var selection = _transformer.FilterContainerByInterfaceImplementation(container, interfaceSelector.InterfaceName, invert);

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

        public string Execute(LocationNode node)
        {
            return node.Value.ToString();
        }

        public void Execute(MappingSelectorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(ModificationStatementNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(ModificationTypeNode node, JContainer container)
        {
            throw new NotImplementedException();
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
            var selection = new SelectionResult
            {
                Container = container
            };

            if (node.Operator == ModifierOperator.NOT)
            {
                selection = _transformer.FilterFunctionsByModifier(container, node.Left.ModifierName, true);
            }
            else if (node.Operator == ModifierOperator.AND)
            {
                selection = _transformer.FilterFunctionsByAllModifiers(container, new List<string>()
                    {
                        node.Left.ModifierName,
                        node.Right.ModifierName
                    });
            }
            else if (node.Operator == ModifierOperator.OR)
            {
                selection = _transformer.FilterFunctionsByEitherModifiers(container, new List<string>()
                    {
                        node.Left.ModifierName,
                        node.Right.ModifierName
                    });
            }
            else if (node.Operator == ModifierOperator.NONE)
            {
                selection = _transformer.FilterFunctionsByModifier(container, node.Left.ModifierName, false);
            }

            return selection;
        }

        /// <summary>
        /// Filter the container to find contracts whose name matches ContractName passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(NameContractSelectorNode node, JContainer container)
        {
            var selction = _transformer.FilterContractsByContractName(container, node.ContractName);

            return selction;
        }

        /// <summary>
        /// Filter the container to find contracts containing functions whose name matches FunctionName passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(NameFunctionSelectorNode node, JContainer container)
        {
            var selction = _transformer.FilterFunctionsByFunctionName(container, node.FunctionName);

            return selction;
        }

        public void Execute(NameInstanceSelectorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(NameInterfaceSelectorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(NamePropertySelectorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(NameVariableNameSelectorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(ParameterNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(PlacementNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(PropertySelectorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(ReferenceDefinitionSyntaxNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(ReturnSelectorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(SenderNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(TaggedDefinitionDecoratorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(TypeVariableTypeSelectorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Filter the container to find statements using variables with a visibility mathcing VariableVisibility passed in node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        public SelectionResult Execute(VariableDecoratorNode node, JContainer container)
        {
            var selection = _transformer.FilterVariableGettersByVisibility(container, node.VariableVisibility.ToString().ToLower());

            return selection;
        }

        public void Execute(VariableNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        public void Execute(VariableSelectorNode node, JContainer container)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A wildcard selector node is executed by returning the wildcard delimiter
        /// </summary>
        /// <param name="node"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public string Execute(WildcardSelectorNode node, JContainer container)
        {
            return "*";
        }
    }
}