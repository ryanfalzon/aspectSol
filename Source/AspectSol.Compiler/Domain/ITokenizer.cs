using System.Collections.Generic;

namespace AspectSol.Compiler.Domain
{
    public interface ITokenizer
    {
        List<string> Start(string filepath);
    }
}