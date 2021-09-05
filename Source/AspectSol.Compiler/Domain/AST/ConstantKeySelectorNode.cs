namespace AspectSol.Compiler.Domain.AST
{
    public class ConstantKeySelectorNode : KeySelectorNode
    {
        public string Constant { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}