namespace AspectSol.Compiler.Models.AST
{
    public class NameInstanceSelectorNode : InstanceSelectorNode
    {
        public string InstanceName { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}