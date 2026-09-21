using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>
/// Response/request body shared by the registration-questions endpoints for meetings, webinars, and
/// recordings ({resource}/{id}/registrants/questions).
/// </summary>
public class RegistrationQuestions
{
    [JsonPropertyName("custom_questions")]
    public List<CustomRegistrationQuestion>? CustomQuestions { get; set; }

    [JsonPropertyName("questions")]
    public List<StandardRegistrationQuestion>? Questions { get; set; }
}

public class CustomRegistrationQuestion
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("required")]
    public bool? Required { get; set; }

    [JsonPropertyName("answers")]
    public List<string>? Answers { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}

public class StandardRegistrationQuestion
{
    [JsonPropertyName("field_name")]
    public string? FieldName { get; set; }

    [JsonPropertyName("required")]
    public bool? Required { get; set; }
}
