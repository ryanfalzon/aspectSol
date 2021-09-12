using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class InterfaceSyntaxNode : SyntaxNode
    {
        public InterfaceNode InterfaceNode { get; set; }

        public InterfaceSelectorNode InterfaceSelector { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<InterfaceSyntaxNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine(InterfaceNode.ToString());
            stringBuilder.AppendLine(InterfaceSelector.ToString());

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</InterfaceSyntaxNode>");

            return stringBuilder.ToString();
        }
    }
}