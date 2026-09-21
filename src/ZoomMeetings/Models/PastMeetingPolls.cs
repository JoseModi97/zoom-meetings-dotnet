using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /past_meetings/{meetingId}/polls.</summary>
public class PastMeetingPollResults
{
    [JsonPropertyName("id")]
    public long? Id { get; set; }

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }

    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("questions")]
    public List<PastMeetingPollParticipantQuestion>? Questions { get; set; }
}

public class PastMeetingPollParticipantQuestion
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("question_details")]
    public List<PastMeetingPollQuestionDetail>? QuestionDetails { get; set; }
}

public class PastMeetingPollQuestionDetail
{
    [JsonPropertyName("question")]
    public string? Question { get; set; }

    [JsonPropertyName("answer")]
    public string? Answer { get; set; }

    [JsonPropertyName("date_time")]
    public string? DateTime { get; set; }

    [JsonPropertyName("polling_id")]
    public string? PollingId { get; set; }
}
