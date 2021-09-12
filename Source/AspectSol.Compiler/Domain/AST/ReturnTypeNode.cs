using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class ReturnTypeNode : Node
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<ReturnTypeNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine($"{GetIndentation()}<Type>{Type}</Type>");
            stringBuilder.AppendLine($"{GetIndentation()}<Name>{Name}</Name>");

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</ReturnTypeNode>");

            return stringBuilder.ToString();
        }
    }
}