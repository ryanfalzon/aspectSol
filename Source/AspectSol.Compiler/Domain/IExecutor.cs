using AspectSol.Compiler.Domain.AST;
using AspectSol.Compiler.Infra.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AspectSol.Compiler.Domain
{
    public interface IExecutor
    {
        void Start(List<StatementNode> statements, JContainer container);

        SelectionResult Execute(StatementNode node, JContainer container);
        SelectionResult Execute(AddressContractSelectorNode node, JContainer container);
        SelectionResult Execute(AppendStatementNode node, JContainer container);
        SelectionResult Execute(ConstantKeySelectorNode node, JContainer container);
        SelectionResult Execute(DefinitionSelectorNode node, JContainer container);
        SelectionResult Execute(DictionaryElementVariableNameSelectoreNode node, JContainer container);
        SelectionResult Execute(InstanceDefinitionSyntaxNode node, JContainer container);
        SelectionResult Execute(InterfaceContractSelectorNode node, JContainer container);
        SelectionResult Execute(InterfaceSyntaxNode node, JContainer container);
        SelectionResult Execute(MappingSelectorNode node, JContainer container);
        SelectionResult Execute(ModificationStatementNode node, JContainer container);
        SelectionResult Execute(ModifierNode node, JContainer container);
        SelectionResult Execute(ModifierSyntaxNode node, JContainer container);
        SelectionResult Execute(NameContractSelectorNode node, JContainer container);
        SelectionResult Execute(NameFunctionSelectorNode node, JContainer container);
        SelectionResult Execute(NameInterfaceSelectorNode node, JContainer container);
        SelectionResult Execute(NamePropertySelectorNode node, JContainer container);
        SelectionResult Execute(NameVariableNameSelectorNode node, JContainer container);
        SelectionResult Execute(ParameterNode node, JContainer container);
        SelectionResult Execute(ReferenceDefinitionSyntaxNode node, JContainer container);
        SelectionResult Execute(ReturnSelectorNode node, JContainer container);
        SelectionResult Execute(ReturnTypeNode node, JContainer container);
        SelectionResult Execute(SenderNode node, JContainer container);
        SelectionResult Execute(TaggedDefinitionDecoratorNode node, JContainer container);
        SelectionResult Execute(TypeVariableTypeSelectorNode node, JContainer container);
        SelectionResult Execute(VariableDecoratorNode node, JContainer container);
        SelectionResult Execute(VariableSelectorNode node, JContainer container);
        SelectionResult Execute(SelectorNode node, JContainer container);
        void Execute(AspectExpressionNode node, JContainer container);
        void Execute(AspectAddTagExpressionNode node, JContainer container);
        void Execute(AspectGenericExpressionNode node, JContainer container);
        void Execute(AspectImplementExpressionNode node, JContainer container);
        void Execute(AspectRemoveTagExpressionNode node, JContainer container);
        void Execute(AspectUpdateTagExpressionNode node, JContainer container);
    }
}