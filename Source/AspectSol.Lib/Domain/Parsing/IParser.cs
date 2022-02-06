using AspectSol.Lib.Domain.AST;
using AspectSol.Lib.Domain.Tokenization;
using System.Collections.Generic;

namespace AspectSol.Lib.Domain.Parsing;

public interface IParser
{
    AspectNode Parse(List<DslToken> tokens);
}