using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /users/{userId}/webinar_templates.</summary>
public class ListWebinarTemplatesResult
{
    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("templates")]
    public List<WebinarTemplate>? Templates { get; set; }
}

public class WebinarTemplate
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }
}

/// <summary>Request body for POST /users/{userId}/webinar_templates.</summary>
public class CreateWebinarTemplateRequest
{
    [JsonPropertyName("webinar_id")]
    public long WebinarId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("save_recurrence")]
    public bool? SaveRecurrence { get; set; }

    [JsonPropertyName("overwrite")]
    public bool? Overwrite { get; set; }
}
