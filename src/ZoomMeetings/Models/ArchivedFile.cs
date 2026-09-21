using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /archive_files.</summary>
public class ListArchivedFilesResult
{
    [JsonPropertyName("from")]
    public DateTimeOffset? From { get; set; }

    [JsonPropertyName("to")]
    public DateTimeOffset? To { get; set; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("meetings")]
    public List<ArchivedFileSet>? Meetings { get; set; }
}

/// <summary>
/// One meeting's set of archived files - shared by GET /archive_files (list) and
/// GET /past_meetings/{meetingUUID}/archive_files (single).
/// </summary>
public class ArchivedFileSet
{
    [JsonPropertyName("account_name")]
    public string? AccountName { get; set; }

    [JsonPropertyName("uuid")]
    public string? Uuid { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("host_id")]
    public string? HostId { get; set; }

    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("complete_time")]
    public DateTimeOffset? CompleteTime { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("duration_in_second")]
    public int? DurationInSecond { get; set; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }

    [JsonPropertyName("total_size")]
    public long? TotalSize { get; set; }

    [JsonPropertyName("recording_count")]
    public int? RecordingCount { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("is_breakout_room")]
    public bool? IsBreakoutRoom { get; set; }

    [JsonPropertyName("parent_meeting_id")]
    public string? ParentMeetingId { get; set; }

    [JsonPropertyName("meeting_type")]
    public string? MeetingType { get; set; }

    [JsonPropertyName("group_id")]
    public string? GroupId { get; set; }

    [JsonPropertyName("archive_files")]
    public List<ArchiveFile>? ArchiveFiles { get; set; }

    [JsonPropertyName("physical_files")]
    public List<ArchivePhysicalFile>? PhysicalFiles { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}

public class ArchiveFile
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("file_type")]
    public string? FileType { get; set; }

    [JsonPropertyName("file_extension")]
    public string? FileExtension { get; set; }

    [JsonPropertyName("file_size")]
    public long? FileSize { get; set; }

    [JsonPropertyName("download_url")]
    public string? DownloadUrl { get; set; }

    [JsonPropertyName("file_path")]
    public string? FilePath { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("recording_type")]
    public string? RecordingType { get; set; }

    [JsonPropertyName("individual")]
    public bool? Individual { get; set; }

    [JsonPropertyName("participant_email")]
    public string? ParticipantEmail { get; set; }

    [JsonPropertyName("participant_join_time")]
    public DateTimeOffset? ParticipantJoinTime { get; set; }

    [JsonPropertyName("participant_leave_time")]
    public DateTimeOffset? ParticipantLeaveTime { get; set; }

    [JsonPropertyName("encryption_fingerprint")]
    public string? EncryptionFingerprint { get; set; }

    [JsonPropertyName("number_of_messages")]
    public int? NumberOfMessages { get; set; }

    [JsonPropertyName("storage_location")]
    public string? StorageLocation { get; set; }

    [JsonPropertyName("auto_delete")]
    public bool? AutoDelete { get; set; }
}

public class ArchivePhysicalFile
{
    [JsonPropertyName("file_id")]
    public string? FileId { get; set; }

    [JsonPropertyName("file_name")]
    public string? FileName { get; set; }

    [JsonPropertyName("file_size")]
    public long? FileSize { get; set; }

    [JsonPropertyName("download_url")]
    public string? DownloadUrl { get; set; }
}
