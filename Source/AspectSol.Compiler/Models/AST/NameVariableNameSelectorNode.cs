namespace AspectSol.Compiler.Models.AST
{
    public class NameVariableNameSelectorNode : VariableNameSelectorNode
    {
        public string VariableName { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}