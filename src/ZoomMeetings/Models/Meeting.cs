using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>
/// A Zoom meeting. Only the commonly-used fields are typed; Zoom returns many more (the full shape
/// isn't in components.schemas in the spec — it's a large inline object). Anything not modeled here
/// is preserved in <see cref="ExtraData"/> instead of being silently dropped.
/// </summary>
public class Meeting
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }

    [JsonPropertyName("host_id")]
    public string? HostId { get; set; }

    [JsonPropertyName("host_email")]
    public string? HostEmail { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }

    [JsonPropertyName("agenda")]
    public string? Agenda { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonPropertyName("join_url")]
    public string? JoinUrl { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("encrypted_password")]
    public string? EncryptedPassword { get; set; }

    [JsonPropertyName("pmi")]
    public string? Pmi { get; set; }

    [JsonPropertyName("registration_url")]
    public string? RegistrationUrl { get; set; }

    [JsonPropertyName("settings")]
    public MeetingSettings? Settings { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}
