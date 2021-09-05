using AspectSol.Compiler.Infra.Enums;

namespace AspectSol.Compiler.Infra.Models
{
    public class Token
    {
        public TokenType Type { get; set; }

        public string Value { get; set; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}