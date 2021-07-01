namespace AspectSol.Compiler.Models.AST
{
    public class MappingSelectorNode : SelectorNode
    {
        public string MappingName { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}