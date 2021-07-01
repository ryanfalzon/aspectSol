namespace AspectSol.Compiler.Models.AST
{
    public class InterfaceContractSelectorNode : ContractSelectorNode
    {
        public string ContractName { get; set; }

        public string InterfaceName { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}