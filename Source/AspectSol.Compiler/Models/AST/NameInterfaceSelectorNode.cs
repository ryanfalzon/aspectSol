namespace AspectSol.Compiler.Models.AST
{
    public class NameInterfaceSelectorNode : InterfaceSelectorNode
    {
        public string InterfaceName { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}