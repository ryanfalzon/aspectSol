namespace AspectSol.Compiler.Models.AST
{
    public class ParameterNode : Node
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}