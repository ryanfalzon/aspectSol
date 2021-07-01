namespace AspectSol.Compiler.Models.AST
{
    public class AddressContractSelectorNode : SelectorNode
    {
        public string ContractAddress { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}