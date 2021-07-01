namespace AspectSol.Compiler.Models.AST
{
    public class InterfaceSyntaxNode : SyntaxNode
    {
        public InterfaceNode InterfaceNode { get; set; }

        public InterfaceSelectorNode InterfaceSelector { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}