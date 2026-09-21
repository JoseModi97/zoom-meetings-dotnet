using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>GET /meetings/{meetingId}/recordings - get a meeting's cloud recordings.</summary>
    public Task<RecordingSet?> GetMeetingRecordingsAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<RecordingSet>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/recordings", cancellationToken: cancellationToken);

    /// <summary>DELETE /meetings/{meetingId}/recordings - delete all of a meeting's recordings ("trash" by default, "delete" to permanently remove).</summary>
    public Task DeleteMeetingRecordingsAsync(string meetingId, string? action = null, CancellationToken cancellationToken = default)
    {
        var query = action == null ? null : new Dictionary<string, string?> { ["action"] = action };
        return CallAsync(HttpMethod.Delete, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/recordings", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>DELETE /meetings/{meetingId}/recordings/{recordingId} - delete a single recording file.</summary>
    public Task DeleteRecordingFileAsync(string meetingId, string recordingId, string? action = null, CancellationToken cancellationToken = default)
    {
        var query = action == null ? null : new Dictionary<string, string?> { ["action"] = action };
        return CallAsync(HttpMethod.Delete, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/recordings/{Uri.EscapeDataString(recordingId)}", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /meetings/{meetingId}/recordings/settings - get a meeting's recording settings.</summary>
    public Task<RecordingSettings?> GetRecordingSettingsAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<RecordingSettings>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/recordings/settings", cancellationToken: cancellationToken);

    /// <summary>PATCH /meetings/{meetingId}/recordings/settings - update a meeting's recording settings.</summary>
    public Task UpdateRecordingSettingsAsync(string meetingId, RecordingSettings settings, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/recordings/settings", settings, cancellationToken: cancellationToken);
}
