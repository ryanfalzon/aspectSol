using Newtonsoft.Json;

namespace AspectSol.Lib.Domain.JavascriptExecution;

public class JavascriptResponse
{
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("data")]
    public string Data { get; set; }
}