using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Request body for adding a meeting or webinar registrant.</summary>
public class AddRegistrantRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("comments")]
    public string? Comments { get; set; }
}

/// <summary>Response body for adding a meeting or webinar registrant.</summary>
public class AddRegistrantResult
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("registrant_id")]
    public string? RegistrantId { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("join_url")]
    public string? JoinUrl { get; set; }
}
