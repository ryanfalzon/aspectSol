using AspectSol.Lib.Domain.AST;
using AspectSol.Lib.Domain.Tokenization;

namespace AspectSol.Lib.Domain.Parsing;

public interface IParser
{
    AspectNode Parse(List<DslToken> tokens);
}