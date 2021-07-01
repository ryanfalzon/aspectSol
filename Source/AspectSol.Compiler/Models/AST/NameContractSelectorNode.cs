namespace AspectSol.Compiler.Models.AST
{
    public class NameContractSelectorNode : ContractSelectorNode
    {
        public string ContractName { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}