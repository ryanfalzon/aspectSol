namespace AspectSol.Compiler.Infra.Models
{
    public class TokenMatch
    {
        public bool IsMatch { get; set; }

        public Token Token { get; set; }

        public string RemainingText { get; set; }

        public TokenMatch()
        {
            IsMatch = false;
        }
    }
}