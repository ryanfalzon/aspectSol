namespace AspectSol.Compiler.Domain.AST
{
    public class ModifierNode : Node
    {
        public string ModifierName { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}