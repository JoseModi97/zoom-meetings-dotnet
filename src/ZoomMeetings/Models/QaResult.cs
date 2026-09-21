using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>
/// Response body shared by the Q&amp;A endpoints for both meetings and webinars
/// (past_meetings/.../qa, past_webinars/.../qa, report/.../qa).
/// </summary>
public class QaResult
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }

    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("questions")]
    public List<QaQuestion>? Questions { get; set; }
}

public class QaQuestion
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("question_details")]
    public List<QaQuestionDetail>? QuestionDetails { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}

public class QaQuestionDetail
{
    [JsonPropertyName("question")]
    public string? Question { get; set; }

    [JsonPropertyName("answer")]
    public string? Answer { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}
