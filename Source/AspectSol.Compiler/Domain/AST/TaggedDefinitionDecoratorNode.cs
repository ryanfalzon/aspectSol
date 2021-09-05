namespace AspectSol.Compiler.Domain.AST
{
    public class TaggedDefinitionDecoratorNode : DefinitionDecoratorNode
    {
        public ModifierSyntaxNode ModifierSyntax { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}