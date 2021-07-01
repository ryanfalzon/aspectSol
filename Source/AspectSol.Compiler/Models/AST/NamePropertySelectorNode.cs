namespace AspectSol.Compiler.Models.AST
{
    public class NamePropertySelectorNode : PropertySelectorNode
    {
        public string PropertyName { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}