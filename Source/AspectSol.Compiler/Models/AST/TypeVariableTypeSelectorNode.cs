namespace AspectSol.Compiler.Models.AST
{
    public class TypeVariableTypeSelectorNode : VariableTypeSelectorNode
    {
        public string Type { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}