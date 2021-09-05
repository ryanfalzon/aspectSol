using AspectSol.Compiler.Infra.Enums;

namespace AspectSol.Compiler.Domain.AST
{
    public class ModifierSyntaxNode : SyntaxNode
    {
        public ModifierOperator Operator { get; set; }

        public ModifierNode Left { get; set; }

        public ModifierNode Right { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}