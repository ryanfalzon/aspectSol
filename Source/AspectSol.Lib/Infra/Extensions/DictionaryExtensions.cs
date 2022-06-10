namespace AspectSol.Lib.Infra.Extensions;

public static class DictionaryExtensions
{
    public static Dictionary<TKey, TValue> SafeConcat<TKey, TValue>(this Dictionary<TKey, TValue> dictionaryA, Dictionary<TKey, TValue> dictionaryB)
    {
        if (dictionaryA == null || dictionaryA.Count == 0) return dictionaryB;
        if (dictionaryB == null || dictionaryB.Count == 0) return dictionaryB;
        
        var dictionary = dictionaryA.ToDictionary(item => item.Key, item => item.Value);

        foreach (var item in 
                 dictionaryB.Where(item => !(dictionary.ContainsKey(item.Key) && dictionary[item.Key].Equals(item.Value))))
        {
            dictionary.Add(item.Key, item.Value);
        }

        return dictionary;
    }

    public static Dictionary<TKey, TValue> SafetIntersect<TKey, TValue>(this Dictionary<TKey, TValue> dictionaryA, Dictionary<TKey, TValue> dictionaryB)
    {
        if (dictionaryA == null || dictionaryA.Count == 0) return dictionaryB;
        if (dictionaryB == null || dictionaryB.Count == 0) return dictionaryB;

        var dictionary = dictionaryA.Intersect(dictionaryB).ToDictionary(item => item.Key, item => item.Value);
        return dictionary;
    }

    public static Dictionary<TKey, TValue> SafeUnion<TKey, TValue>(this Dictionary<TKey, TValue> dictionaryA, Dictionary<TKey, TValue> dictionaryB)
    {
        if (dictionaryA == null || dictionaryA.Count == 0) return dictionaryB;
        if (dictionaryB == null || dictionaryB.Count == 0) return dictionaryB;

        var dictionary = dictionaryA.Union(dictionaryB).ToDictionary(item => item.Key, item => item.Value);
        return dictionary;
    }
}