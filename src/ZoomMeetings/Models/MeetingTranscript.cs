using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /meetings/{meetingId}/transcript.</summary>
public class MeetingTranscript
{
    [JsonPropertyName("meeting_id")]
    public string? MeetingId { get; set; }

    [JsonPropertyName("account_id")]
    public string? AccountId { get; set; }

    [JsonPropertyName("meeting_topic")]
    public string? MeetingTopic { get; set; }

    [JsonPropertyName("host_id")]
    public string? HostId { get; set; }

    [JsonPropertyName("transcript_created_time")]
    public DateTimeOffset? TranscriptCreatedTime { get; set; }

    [JsonPropertyName("can_download")]
    public bool? CanDownload { get; set; }

    [JsonPropertyName("auto_delete")]
    public bool? AutoDelete { get; set; }

    [JsonPropertyName("auto_delete_date")]
    public DateTimeOffset? AutoDeleteDate { get; set; }

    [JsonPropertyName("download_url")]
    public string? DownloadUrl { get; set; }

    /// <summary>"DELETED_OR_TRASHED" | "UNSUPPORTED" | "NO_TRANSCRIPT_DATA" | "NOT_READY", when download_url isn't available.</summary>
    [JsonPropertyName("download_restriction_reason")]
    public string? DownloadRestrictionReason { get; set; }
}
