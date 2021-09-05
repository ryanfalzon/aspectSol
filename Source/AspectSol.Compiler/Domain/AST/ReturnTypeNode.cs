namespace AspectSol.Compiler.Domain.AST
{
    public class ReturnTypeNode : Node
    {
        public string Type { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}