using System.Threading.Tasks;

namespace AspectSol.Compiler.Domain
{
    public interface IContractParser
    {
        Task<string> Parse(string input);
    }
}