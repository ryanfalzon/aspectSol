using AspectSol.Lib.Domain.Ast;
using AspectSol.Lib.Domain.Tokenizer;

namespace AspectSol.Lib.Domain.Parser;

public interface IParser
{
    AspectNode Parse(List<DslToken> tokens);
}