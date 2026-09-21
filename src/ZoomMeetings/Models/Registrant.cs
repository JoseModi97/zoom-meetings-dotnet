using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>
/// A meeting or webinar registrant. Reused by both ZoomClient.Registrants.cs (meetings) and
/// ZoomClient.Webinars.cs (webinars) — the request/response shapes match closely enough across the
/// two resource types in Zoom's spec that a single set of models covers both.
/// </summary>
public class Registrant
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("registrant_id")]
    public string? RegistrantId { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("create_time")]
    public DateTimeOffset? CreateTime { get; set; }

    [JsonPropertyName("join_url")]
    public string? JoinUrl { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}
