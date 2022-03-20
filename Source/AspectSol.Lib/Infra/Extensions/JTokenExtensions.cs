using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Infra.Extensions;

public static class JTokenExtensions
{
    public static bool Matches(this JToken jToken, string value)
    {
        return jToken?.Value<string>() != null && jToken.Value<string>()!.Equals(value);
    }

    public static bool IsTrue(this JToken jToken)
    {
        return jToken.Value<bool>();
    }

    public static bool IsFalse(this JToken jToken)
    {
        return !jToken.Value<bool>();
    }

    public static bool IsValueNullOrWhitespace(this JToken jToken)
    {
        return jToken is not {HasValues: true};
    }

    public static List<JToken> ToSafeList(this JToken jToken)
    {
        return jToken.Value<JArray>() == null ? new List<JToken>() : jToken.Value<JArray>()!.ToList();
    }
}