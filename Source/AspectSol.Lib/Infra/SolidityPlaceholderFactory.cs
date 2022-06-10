namespace AspectSol.Lib.Infra;

public class SolidityPlaceholderFactory
{
    private const string StatementsPlaceholder =
        "pragma solidity >=0.7.0 <0.9.0;" +
        "contract TestContract {" +
        "   function TestFunction() public {" +
        "       {Statements}" +
        "   }" +
        "}";

    public static string GetStatementPlaceholder(string statement)
    {
        return StatementsPlaceholder.Replace("{Statements}", statement);
    }

    public static string GetStatementsPlaceholder(IEnumerable<string> statements)
    {
        return StatementsPlaceholder.Replace("{Statements}", string.Join('\n', statements));
    }
}