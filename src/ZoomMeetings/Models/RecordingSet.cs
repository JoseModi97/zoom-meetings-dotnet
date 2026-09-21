using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for ZoomClient.GetMeetingRecordingsAsync (GET /meetings/{meetingId}/recordings).</summary>
public class RecordingSet
{
    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("account_id")]
    public string? AccountId { get; set; }

    [JsonPropertyName("host_id")]
    public string? HostId { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("total_size")]
    public long? TotalSize { get; set; }

    [JsonPropertyName("recording_count")]
    public int? RecordingCount { get; set; }

    [JsonPropertyName("recording_files")]
    public List<RecordingFile>? RecordingFiles { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}

public class RecordingFile
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("meeting_id")]
    public string? MeetingId { get; set; }

    [JsonPropertyName("recording_start")]
    public DateTimeOffset? RecordingStart { get; set; }

    [JsonPropertyName("recording_end")]
    public DateTimeOffset? RecordingEnd { get; set; }

    [JsonPropertyName("file_type")]
    public string? FileType { get; set; }

    [JsonPropertyName("file_size")]
    public long? FileSize { get; set; }

    [JsonPropertyName("play_url")]
    public string? PlayUrl { get; set; }

    [JsonPropertyName("download_url")]
    public string? DownloadUrl { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("recording_type")]
    public string? RecordingType { get; set; }
}
