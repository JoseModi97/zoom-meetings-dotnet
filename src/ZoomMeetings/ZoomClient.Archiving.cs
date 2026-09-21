using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

/// <summary>Archiving (meeting/webinar content archival for compliance) operations.</summary>
public sealed partial class ZoomClient
{
    /// <summary>GET /archive_files - list a single page of an account's archived files.</summary>
    public Task<ListArchivedFilesResult?> ListArchivedFilesAsync(
        int? pageSize = null, string? nextPageToken = null, DateTimeOffset? from = null, DateTimeOffset? to = null, string? groupId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (from != null) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to != null) query["to"] = to.Value.ToString("yyyy-MM-dd");
        if (groupId != null) query["group_id"] = groupId;
        return CallAsync<ListArchivedFilesResult>(HttpMethod.Get, "/archive_files", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /archive_files/download_audit - list who downloaded which archived files, and when.</summary>
    public Task<ListArchiveFileDownloadAuditResult?> ListArchiveFileDownloadAuditAsync(
        int? pageSize = null, string? nextPageToken = null, DateTimeOffset? from = null, DateTimeOffset? to = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (from != null) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to != null) query["to"] = to.Value.ToString("yyyy-MM-dd");
        return CallAsync<ListArchiveFileDownloadAuditResult>(HttpMethod.Get, "/archive_files/download_audit", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /archive_files/statistics - archived-file counts broken down by extension and status.</summary>
    public Task<ArchivedFileStatistics?> GetArchivedFileStatisticsAsync(DateTimeOffset? from = null, DateTimeOffset? to = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (from != null) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to != null) query["to"] = to.Value.ToString("yyyy-MM-dd");
        return CallAsync<ArchivedFileStatistics>(HttpMethod.Get, "/archive_files/statistics", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>PATCH /archive_files/{fileId} - set whether an archived file auto-deletes.</summary>
    public Task UpdateArchivedFileAutoDeleteAsync(string fileId, bool autoDelete, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/archive_files/{Uri.EscapeDataString(fileId)}", new { auto_delete = autoDelete }, cancellationToken: cancellationToken);

    /// <summary>GET /past_meetings/{meetingUUID}/archive_files - get a specific meeting's archived files.</summary>
    public Task<ArchivedFileSet?> GetMeetingArchivedFilesAsync(string meetingUuid, CancellationToken cancellationToken = default)
        => CallAsync<ArchivedFileSet>(HttpMethod.Get, $"/past_meetings/{ZoomIdEncoding.EncodePathSegment(meetingUuid)}/archive_files", cancellationToken: cancellationToken);

    /// <summary>DELETE /past_meetings/{meetingUUID}/archive_files - delete a specific meeting's archived files.</summary>
    public Task DeleteMeetingArchivedFilesAsync(string meetingUuid, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/past_meetings/{ZoomIdEncoding.EncodePathSegment(meetingUuid)}/archive_files", cancellationToken: cancellationToken);
}
