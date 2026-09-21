using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>A tracking field definition.</summary>
public class TrackingField
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("field")]
    public string? Field { get; set; }

    [JsonPropertyName("required")]
    public bool? Required { get; set; }

    [JsonPropertyName("visible")]
    public bool? Visible { get; set; }

    [JsonPropertyName("recommended_values")]
    public List<string>? RecommendedValues { get; set; }
}

/// <summary>Response body for GET /tracking_fields.</summary>
public class ListTrackingFieldsResult
{
    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("tracking_fields")]
    public List<TrackingField>? TrackingFields { get; set; }
}

/// <summary>Request body for POST /tracking_fields.</summary>
public class CreateTrackingFieldRequest
{
    [JsonPropertyName("field")]
    public string Field { get; set; } = string.Empty;

    [JsonPropertyName("required")]
    public bool? Required { get; set; }

    [JsonPropertyName("visible")]
    public bool? Visible { get; set; }

    [JsonPropertyName("recommended_values")]
    public List<string>? RecommendedValues { get; set; }
}

/// <summary>Request body for PATCH /tracking_fields/{fieldId}.</summary>
public class UpdateTrackingFieldRequest
{
    [JsonPropertyName("field")]
    public string? Field { get; set; }

    [JsonPropertyName("required")]
    public bool? Required { get; set; }

    [JsonPropertyName("visible")]
    public bool? Visible { get; set; }

    [JsonPropertyName("recommended_values")]
    public List<string>? RecommendedValues { get; set; }
}
