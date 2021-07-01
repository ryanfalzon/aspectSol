namespace AspectSol.Compiler.Models.AST
{
    public class ModificationStatementNode : StatementNode
    {
        public ModificationTypeNode ModificationType { get; set; }

        public ReferenceDefinitionSyntaxNode ReferenceDefinition { get; set; }

        public DefinitionDecoratorNode DefinitionDecorator { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}