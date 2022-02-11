namespace AspectSol.Lib.Domain.JavascriptExecution;

public class ScriptFactory
{
    public async Task<string> GetScript(string scriptName)
    {
        var script = scriptName switch
        {
            "GenerateAST" => await File.ReadAllTextAsync($"scripts/{scriptName}.js"),
            "GetPartialASTForFunction" => await File.ReadAllTextAsync($"scripts/{scriptName}.js"),
            _ => throw new FileNotFoundException($"Script with the name '{scriptName}' could not be found")
        };

        return script;
    }
}