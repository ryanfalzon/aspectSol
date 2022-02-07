namespace AspectSol.Lib.Domain.Tokenization;

public interface ITokenizer
{
    List<DslToken> Tokenize(string source, bool verboseLogging = false);
}