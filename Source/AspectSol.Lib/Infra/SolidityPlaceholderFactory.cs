namespace AspectSol.Lib.Infra;

public class SolidityPlaceholderFactory
{
    private const string ContractStatementsPlaceholder =
        "pragma solidity >=0.7.0 <0.9.0;\n" +
        "contract TestContract {\n" +
        "   {ContractStatements}\n" +
        "}";
    
    private const string FunctionStatementsPlaceholder =
        "pragma solidity >=0.7.0 <0.9.0;\n" +
        "contract TestContract {\n" +
        "   {ContractStatements}\n" +
        "   function TestFunction() public {\n" +
        "       {FunctionStatements}\n" +
        "   }\n" +
        "}";
    
    private static readonly List<string> _contractStatementCache = new();

    public static string GetContractStatementPlaceholder(string statement)
    {
        _contractStatementCache.Add(statement);
        return ContractStatementsPlaceholder.Replace("{ContractStatements}", statement);
    }

    public static string GetFunctionStatementPlaceholder(string statement)
    {
        return FunctionStatementsPlaceholder
            .Replace("{ContractStatements}", string.Join('\n', _contractStatementCache))
            .Replace("{FunctionStatements}", statement);
    }
}