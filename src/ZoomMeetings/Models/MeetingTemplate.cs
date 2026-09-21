using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /users/{userId}/meeting_templates.</summary>
public class ListMeetingTemplatesResult
{
    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("templates")]
    public List<MeetingTemplateItem>? Templates { get; set; }
}

public class MeetingTemplateItem
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }
}

/// <summary>Request body for POST /users/{userId}/meeting_templates.</summary>
public class CreateMeetingTemplateRequest
{
    [JsonPropertyName("meeting_id")]
    public string MeetingId { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("save_recurrence")]
    public bool? SaveRecurrence { get; set; }

    [JsonPropertyName("overwrite")]
    public bool? Overwrite { get; set; }
}

/// <summary>Response body for POST /users/{userId}/meeting_templates.</summary>
public class CreateMeetingTemplateResult
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
