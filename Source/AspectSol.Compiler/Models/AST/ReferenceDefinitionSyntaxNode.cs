namespace AspectSol.Compiler.Models.AST
{
    public class ReferenceDefinitionSyntaxNode : DefinitionSyntaxNode
    {
        public SelectorNode ContractSelector { get; set; }

        public SelectorNode FunctionSelector { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}