namespace AspectSol.Compiler.Models.AST
{
    public class PropertySelectorNode : SelectorNode
    {
        public PropertySelectorNode Property { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}