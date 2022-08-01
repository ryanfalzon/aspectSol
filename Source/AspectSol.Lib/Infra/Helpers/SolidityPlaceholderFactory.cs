namespace AspectSol.Lib.Infra.Helpers;

/// <summary>
/// A factory that places definitions and statements within the context of solidity smart contract and function code
/// Mainly used to easily create AST objects of new code needed to be added to a smart contract
/// </summary>
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
    
    private static readonly List<string> ContractStatementCache = new();

    public static string GetContractStatementPlaceholder(string statement)
    {
        ContractStatementCache.Add(statement);
        return ContractStatementsPlaceholder.Replace("{ContractStatements}", statement);
    }

    public static string GetFunctionStatementPlaceholder(string statement)
    {
        return FunctionStatementsPlaceholder
            .Replace("{ContractStatements}", string.Join('\n', ContractStatementCache))
            .Replace("{FunctionStatements}", statement);
    }
}