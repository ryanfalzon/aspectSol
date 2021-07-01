using AspectSol.Compiler.Models.Enums;
using System.Collections.Generic;

namespace AspectSol.Compiler.Models.AST
{
    public class ModifierSyntaxNode : SyntaxNode
    {
        public ModifierOperator Operator { get; set; }

        public List<ModifierSyntaxNode> Modifiers { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}