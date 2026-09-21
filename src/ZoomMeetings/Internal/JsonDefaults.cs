using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Internal;

internal static class JsonDefaults
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance,
        DictionaryKeyPolicy = SnakeCaseNamingPolicy.Instance,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };
}
