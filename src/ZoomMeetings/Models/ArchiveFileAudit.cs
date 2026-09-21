using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /archive_files/download_audit.</summary>
public class ListArchiveFileDownloadAuditResult
{
    [JsonPropertyName("from")]
    public DateTimeOffset? From { get; set; }

    [JsonPropertyName("to")]
    public DateTimeOffset? To { get; set; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("meetings")]
    public List<ArchiveFileDownloadAuditEntry>? Meetings { get; set; }
}

public class ArchiveFileDownloadAuditEntry
{
    [JsonPropertyName("meeting_uuid")]
    public string? MeetingUuid { get; set; }

    [JsonPropertyName("files")]
    public List<ArchiveFileDownloadAuditFile>? Files { get; set; }
}

public class ArchiveFileDownloadAuditFile
{
    [JsonPropertyName("file_id")]
    public string? FileId { get; set; }

    [JsonPropertyName("downloaded_by")]
    public string? DownloadedBy { get; set; }

    [JsonPropertyName("download_time")]
    public DateTimeOffset? DownloadTime { get; set; }
}

/// <summary>Response body for GET /archive_files/statistics.</summary>
public class ArchivedFileStatistics
{
    [JsonPropertyName("from")]
    public DateTimeOffset? From { get; set; }

    [JsonPropertyName("to")]
    public DateTimeOffset? To { get; set; }

    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("statistic_by_file_extension")]
    public ArchiveFileExtensionCounts? StatisticByFileExtension { get; set; }

    [JsonPropertyName("statistic_by_file_status")]
    public ArchiveFileStatusCounts? StatisticByFileStatus { get; set; }
}

public class ArchiveFileExtensionCounts
{
    [JsonPropertyName("mp4_file_count")]
    public int Mp4FileCount { get; set; }

    [JsonPropertyName("m4a_file_count")]
    public int M4aFileCount { get; set; }

    [JsonPropertyName("txt_file_count")]
    public int TxtFileCount { get; set; }

    [JsonPropertyName("json_file_count")]
    public int JsonFileCount { get; set; }

    [JsonPropertyName("vtt_file_count")]
    public int VttFileCount { get; set; }
}

public class ArchiveFileStatusCounts
{
    [JsonPropertyName("processing_file_count")]
    public int ProcessingFileCount { get; set; }

    [JsonPropertyName("completed_file_count")]
    public int CompletedFileCount { get; set; }

    [JsonPropertyName("failed_file_count")]
    public int FailedFileCount { get; set; }
}
