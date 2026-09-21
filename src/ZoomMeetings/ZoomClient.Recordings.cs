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

    /// <summary>PUT /meetings/{meetingId}/recordings/{recordingId}/status - recover (un-delete) a single recording file.</summary>
    public Task RecoverRecordingFileAsync(string meetingId, string recordingId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Put, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/recordings/{Uri.EscapeDataString(recordingId)}/status", new { action = "recover" }, cancellationToken: cancellationToken);

    /// <summary>PUT /meetings/{meetingUUID}/recordings/status - recover (un-delete) all of a meeting's recordings.</summary>
    public Task RecoverMeetingRecordingsAsync(string meetingUuid, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Put, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingUuid)}/recordings/status", new { action = "recover" }, cancellationToken: cancellationToken);

    /// <summary>GET /meetings/{meetingId}/recordings/analytics_details - a meeting recording's viewer-level analytics.</summary>
    public Task<RecordingAnalyticsDetailsResult?> GetRecordingAnalyticsDetailsAsync(
        string meetingId, int? pageSize = null, string? nextPageToken = null, DateTimeOffset? from = null, DateTimeOffset? to = null, string? type = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (from != null) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to != null) query["to"] = to.Value.ToString("yyyy-MM-dd");
        if (type != null) query["type"] = type;
        return CallAsync<RecordingAnalyticsDetailsResult>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/recordings/analytics_details", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /meetings/{meetingId}/recordings/analytics_summary - a meeting recording's daily view/download totals.</summary>
    public Task<RecordingAnalyticsSummaryResult?> GetRecordingAnalyticsSummaryAsync(string meetingId, DateTimeOffset? from = null, DateTimeOffset? to = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (from != null) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to != null) query["to"] = to.Value.ToString("yyyy-MM-dd");
        return CallAsync<RecordingAnalyticsSummaryResult>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/recordings/analytics_summary", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /meetings/{meetingId}/recordings/registrants - list a single page of a recording's registrants.</summary>
    public Task<ListRegistrantsResult?> ListRecordingRegistrantsAsync(
        string meetingId, string? status = null, int? pageSize = null, int? pageNumber = null, string? nextPageToken = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (status != null) query["status"] = status;
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (pageNumber != null) query["page_number"] = pageNumber.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        return CallAsync<ListRegistrantsResult>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/recordings/registrants", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>POST /meetings/{meetingId}/recordings/registrants - add a recording registrant.</summary>
    public Task<AddRegistrantResult?> AddRecordingRegistrantAsync(string meetingId, AddRegistrantRequest request, CancellationToken cancellationToken = default)
        => CallAsync<AddRegistrantResult>(HttpMethod.Post, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/recordings/registrants", request, cancellationToken: cancellationToken);

    /// <summary>GET /meetings/{meetingId}/recordings/registrants/questions - get a recording's registration questions.</summary>
    public Task<RegistrationQuestions?> GetRecordingRegistrationQuestionsAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<RegistrationQuestions>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/recordings/registrants/questions", cancellationToken: cancellationToken);

    /// <summary>PATCH /meetings/{meetingId}/recordings/registrants/questions - update a recording's registration questions.</summary>
    public Task UpdateRecordingRegistrationQuestionsAsync(string meetingId, RegistrationQuestions questions, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/recordings/registrants/questions", questions, cancellationToken: cancellationToken);

    /// <summary>PUT /meetings/{meetingId}/recordings/registrants/status - approve or deny one or more recording registrants.</summary>
    public Task UpdateRecordingRegistrantStatusAsync(string meetingId, string action, IEnumerable<string> registrantIds, CancellationToken cancellationToken = default)
    {
        var body = new { action, registrants = registrantIds.Select(id => new { id }).ToList() };
        return CallAsync(HttpMethod.Put, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/recordings/registrants/status", body, cancellationToken: cancellationToken);
    }

    /// <summary>GET /meetings/{meetingId}/transcript - get a meeting's audio transcript.</summary>
    public Task<MeetingTranscript?> GetMeetingTranscriptAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<MeetingTranscript>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/transcript", cancellationToken: cancellationToken);

    /// <summary>DELETE /meetings/{meetingId}/transcript - delete a meeting's audio transcript.</summary>
    public Task DeleteMeetingTranscriptAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/transcript", cancellationToken: cancellationToken);

    /// <summary>GET /users/{userId}/recordings - list a single page of a user's cloud recordings.</summary>
    public Task<UserRecordingsResult?> ListUserRecordingsAsync(
        string userId, int? pageSize = null, string? nextPageToken = null, bool? trash = null, DateTimeOffset? from = null, DateTimeOffset? to = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (trash != null) query["trash"] = trash.Value.ToString().ToLowerInvariant();
        if (from != null) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to != null) query["to"] = to.Value.ToString("yyyy-MM-dd");
        return CallAsync<UserRecordingsResult>(HttpMethod.Get, $"/users/{Uri.EscapeDataString(userId)}/recordings", query: query, cancellationToken: cancellationToken);
    }
}
