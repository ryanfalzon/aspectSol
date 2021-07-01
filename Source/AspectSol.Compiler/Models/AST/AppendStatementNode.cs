using System.Collections.Generic;

namespace AspectSol.Compiler.Models.AST
{
    public class AppendStatementNode : StatementNode
    {
        public PlacementNode Placement { get; set; }

        public LocationNode Location { get; set; }

        public List<SelectorNode> Selectors { get; set; }

        public SenderNode Sender { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}