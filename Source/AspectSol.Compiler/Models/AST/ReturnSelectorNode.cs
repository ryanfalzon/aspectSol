using System.Collections.Generic;

namespace AspectSol.Compiler.Models.AST
{
    public class ReturnSelectorNode : SelectorNode
    {
        public List<ReturnTypeNode> ReturnTypes { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}