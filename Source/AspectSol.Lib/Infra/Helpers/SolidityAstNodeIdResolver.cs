using System.Text.RegularExpressions;

namespace AspectSol.Lib.Infra.Helpers;

public class SolidityAstNodeIdResolver
{
    private int _nextId;

    public SolidityAstNodeIdResolver()
    {
        _nextId = int.MaxValue;
    }

    public string UpdateNodeIdentifiers(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return string.Empty;

        const string idPropertyPattern = "(\"id\":[0-9]+,)";
        
        json = Regex.Replace(json, idPropertyPattern, m =>
        {
            _nextId--;
            return $"\"id\":{_nextId},";
        });

        return json;
    } 
}