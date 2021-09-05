using AspectSol.Compiler.Infra.Models;
using System.Collections.Generic;

namespace AspectSol.Compiler.Domain
{
    public interface ITokenizer
    {
        List<Token> Start(string filepath);
    }
}