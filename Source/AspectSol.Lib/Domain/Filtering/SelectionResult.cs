using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public class SelectionResult
{
    public JContainer Container { get; set; }

    /// <summary>
    /// A list of interested contracts aspects need to be applied to
    /// </summary>
    public List<string> InterestedContracts { get; set; }

    /// <summary>
    /// Key refers to function name and value is the contract name of where the function resides
    /// </summary>
    public Dictionary<string, string> InterestedFunctions { get; set; }

    /// <summary>
    /// Key refers to definition name and value is the contract name of where the definition resides
    /// </summary>
    public Dictionary<string, string> InterestedDefinitions { get; set; }

    /// <summary>
    /// Key is composed of the function index, the statement index and the argument index if any. The value is the function name of where the statement resides
    /// </summary>
    public Dictionary<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int? ArgumentIndex), string> InterestedStatements { get; set; }
}