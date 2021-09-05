using AspectSol.Compiler.Domain.AST;
using AspectSol.Compiler.Infra.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AspectSol.Compiler.Domain
{
    public interface IExecutor
    {
        string Start(List<StatementNode> statements);

        void Execute(AddressContractSelectorNode node, JContainer container);
        void Execute(AppendStatementNode node, JContainer container);
        SelectionResult Execute(ConstantKeySelectorNode node, JContainer container);
        void Execute(DefinitionSelectorNode node, JContainer container);
        SelectionResult Execute(DictionaryElementVariableNameSelectoreNode node, JContainer container);
        void Execute(ImplementingDefinitionDecoratorNode node, JContainer container);
        void Execute(InstanceDefinitionSyntaxNode node, JContainer container);
        SelectionResult Execute(InterfaceContractSelectorNode node, JContainer container);
        void Execute(InterfaceNode node, JContainer container);
        SelectionResult Execute(InterfaceSyntaxNode node, JContainer container);
        void Execute(LocationNode node, JContainer container);
        void Execute(MappingSelectorNode node, JContainer container);
        void Execute(ModificationStatementNode node, JContainer container);
        void Execute(ModificationTypeNode node, JContainer container);
        SelectionResult Execute(ModifierNode node, JContainer container);
        SelectionResult Execute(ModifierSyntaxNode node, JContainer container);
        SelectionResult Execute(NameContractSelectorNode node, JContainer container);
        SelectionResult Execute(NameFunctionSelectorNode node, JContainer container);
        void Execute(NameInstanceSelectorNode node, JContainer container);
        void Execute(NameInterfaceSelectorNode node, JContainer container);
        void Execute(NamePropertySelectorNode node, JContainer container);
        void Execute(NameVariableNameSelectorNode node, JContainer container);
        void Execute(ParameterNode node, JContainer container);
        void Execute(PlacementNode node, JContainer container);
        void Execute(PropertySelectorNode node, JContainer container);
        void Execute(ReferenceDefinitionSyntaxNode node, JContainer container);
        void Execute(ReturnSelectorNode node, JContainer container);
        void Execute(SenderNode node, JContainer container);
        void Execute(TaggedDefinitionDecoratorNode node, JContainer container);
        void Execute(TypeVariableTypeSelectorNode node, JContainer container);
        SelectionResult Execute(VariableDecoratorNode node, JContainer container);
        void Execute(VariableNode node, JContainer container);
        void Execute(VariableSelectorNode node, JContainer container);
        SelectionResult Execute(WildcardSelectorNode node, JContainer container);
    }
}