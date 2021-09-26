using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AspectSol.Compiler.Infra.Extensions
{
    public static class JTokenExtensions
    {
        public static bool Matches(this JToken jToken, string value)
        {
            return jToken != null && jToken.Value<string>() != null && jToken.Value<string>().Equals(value);
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
            return string.IsNullOrWhiteSpace(jToken.Value<string>());
        }

        public static List<JToken> ToSafeList(this JToken jToken)
        {
            return jToken.Value<JArray>() == null ? new List<JToken>() : jToken.Value<JArray>().ToList();
        }
    }
}