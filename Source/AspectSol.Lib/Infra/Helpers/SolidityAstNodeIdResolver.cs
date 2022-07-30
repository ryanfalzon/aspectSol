using System.Text.RegularExpressions;

namespace AspectSol.Lib.Infra.Helpers;

/// <summary>
/// This class is useful since solc-typed-ast makes use of unique identifiers to reconstruct solidity code from an AST
/// </summary>
public class SolidityAstNodeIdResolver
{
    private int _nextId;

    public SolidityAstNodeIdResolver()
    {
        _nextId = int.MaxValue;
    }

    /// <summary>
    /// Use this method to reevaluate all property object identifiers due to adding new objects to the AST
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
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