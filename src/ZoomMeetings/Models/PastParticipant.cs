using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body shared by GET /past_meetings/{meetingId}/participants and GET /past_webinars/{webinarId}/participants.</summary>
public class ListPastParticipantsResult
{
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("page_count")]
    public int PageCount { get; set; }

    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("participants")]
    public List<PastParticipant>? Participants { get; set; }
}

public class PastParticipant
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    [JsonPropertyName("registrant_id")]
    public string? RegistrantId { get; set; }

    [JsonPropertyName("user_email")]
    public string? UserEmail { get; set; }

    [JsonPropertyName("join_time")]
    public DateTimeOffset? JoinTime { get; set; }

    [JsonPropertyName("leave_time")]
    public DateTimeOffset? LeaveTime { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}
