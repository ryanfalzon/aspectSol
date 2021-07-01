namespace AspectSol.Compiler.Models.AST
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