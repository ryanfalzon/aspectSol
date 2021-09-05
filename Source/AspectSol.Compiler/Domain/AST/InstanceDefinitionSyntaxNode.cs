namespace AspectSol.Compiler.Domain.AST
{
    public class InstanceDefinitionSyntaxNode : DefinitionSyntaxNode
    {
        public NameContractSelectorNode NameContractSelector { get; set; }

        public SelectorNode InstanceSelector { get; set; }

        public SelectorNode FunctionSelector { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}