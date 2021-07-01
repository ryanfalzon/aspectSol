namespace AspectSol.Compiler.Models.AST
{
    public class NameFunctionSelectorNode : FunctionSelectorNode
    {
        public string FunctionName { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}