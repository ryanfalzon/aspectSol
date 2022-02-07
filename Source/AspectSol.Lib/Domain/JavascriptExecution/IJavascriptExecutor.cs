namespace AspectSol.Lib.Domain.JavascriptExecution;

public interface IJavascriptExecutor
{
    Task<string> Execute(string script, object[] arguments);
}