using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for listing meeting or webinar registrants.</summary>
public class ListRegistrantsResult
{
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("registrants")]
    public List<Registrant>? Registrants { get; set; }
}
