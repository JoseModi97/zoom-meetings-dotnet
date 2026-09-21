using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /webinars/{webinarId}/tracking_sources.</summary>
public class ListTrackingSourcesResult
{
    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("tracking_sources")]
    public List<TrackingSource>? TrackingSources { get; set; }
}

public class TrackingSource
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("source_name")]
    public string? SourceName { get; set; }

    [JsonPropertyName("tracking_url")]
    public string? TrackingUrl { get; set; }

    [JsonPropertyName("visitor_count")]
    public int? VisitorCount { get; set; }

    [JsonPropertyName("registration_count")]
    public int? RegistrationCount { get; set; }
}
