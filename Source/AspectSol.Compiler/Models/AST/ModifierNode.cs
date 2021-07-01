namespace AspectSol.Compiler.Models.AST
{
    public class ModifierNode : ModifierSyntaxNode
    {
        public string ModifierName { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}