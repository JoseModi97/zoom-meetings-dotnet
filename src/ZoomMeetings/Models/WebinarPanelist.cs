using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /webinars/{webinarId}/panelists.</summary>
public class ListWebinarPanelistsResult
{
    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("panelists")]
    public List<WebinarPanelist>? Panelists { get; set; }
}

public class WebinarPanelist
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("join_url")]
    public string? JoinUrl { get; set; }

    [JsonPropertyName("virtual_background_id")]
    public string? VirtualBackgroundId { get; set; }

    [JsonPropertyName("name_tag_id")]
    public string? NameTagId { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}

/// <summary>Request body for POST /webinars/{webinarId}/panelists.</summary>
public class AddPanelistsRequest
{
    [JsonPropertyName("panelists")]
    public List<AddPanelistRequest> Panelists { get; set; } = new();
}

public class AddPanelistRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("virtual_background_id")]
    public string? VirtualBackgroundId { get; set; }

    [JsonPropertyName("name_tag_id")]
    public string? NameTagId { get; set; }
}

/// <summary>Response body for POST /webinars/{webinarId}/panelists.</summary>
public class AddPanelistsResult
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
}
