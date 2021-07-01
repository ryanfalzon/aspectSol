namespace AspectSol.Compiler.Models.AST
{
    public class SenderNode : Node
    {
        public ReferenceDefinitionSyntaxNode ReferenceDefinition { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}