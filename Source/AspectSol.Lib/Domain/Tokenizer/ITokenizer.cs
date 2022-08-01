namespace AspectSol.Lib.Domain.Tokenizer;

public interface ITokenizer
{
    List<DslToken> Tokenize(string source);
}