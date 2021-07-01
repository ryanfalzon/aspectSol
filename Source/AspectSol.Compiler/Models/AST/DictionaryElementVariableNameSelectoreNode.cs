namespace AspectSol.Compiler.Models.AST
{
    public class DictionaryElementVariableNameSelectoreNode : VariableNameSelectorNode
    {
        public string VariableName { get; set; }

        public SelectorNode KeySelector { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}