using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class ModificationStatementNode : StatementNode
    {
        public ModificationTypeNode ModificationType { get; set; }

        public ReferenceDefinitionSyntaxNode ReferenceDefinition { get; set; }

        public DefinitionDecoratorNode DefinitionDecorator { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<ModificationStatementNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine(ModificationType.ToString());
            stringBuilder.AppendLine(ReferenceDefinition.ToString());
            stringBuilder.AppendLine(DefinitionDecorator.ToString());
            stringBuilder.AppendLine(base.ToString());

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</ModificationStatementNode>");

            return stringBuilder.ToString();
        }
    }
}