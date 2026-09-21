using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>POST /users/{userId}/meetings - create a scheduled/instant/recurring meeting for a user.</summary>
    public Task<Meeting?> CreateMeetingAsync(string userId, CreateMeetingRequest request, CancellationToken cancellationToken = default)
        => CallAsync<Meeting>(HttpMethod.Post, $"/users/{Uri.EscapeDataString(userId)}/meetings", request, cancellationToken: cancellationToken);

    /// <summary>GET /meetings/{meetingId} - get details for a single meeting.</summary>
    public Task<Meeting?> GetMeetingAsync(string meetingId, string? occurrenceId = null, CancellationToken cancellationToken = default)
    {
        var query = occurrenceId == null ? null : new Dictionary<string, string?> { ["occurrence_id"] = occurrenceId };
        return CallAsync<Meeting>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>PATCH /meetings/{meetingId} - update a meeting. Only set the fields you want to change on <paramref name="request"/>.</summary>
    public Task UpdateMeetingAsync(string meetingId, UpdateMeetingRequest request, string? occurrenceId = null, CancellationToken cancellationToken = default)
    {
        var query = occurrenceId == null ? null : new Dictionary<string, string?> { ["occurrence_id"] = occurrenceId };
        return CallAsync(HttpMethods.Patch, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}", request, query, cancellationToken);
    }

    /// <summary>DELETE /meetings/{meetingId} - delete/cancel a meeting.</summary>
    public Task DeleteMeetingAsync(string meetingId, string? occurrenceId = null, bool? scheduleForReminder = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (occurrenceId != null) query["occurrence_id"] = occurrenceId;
        if (scheduleForReminder != null) query["schedule_for_reminder"] = scheduleForReminder.Value.ToString().ToLowerInvariant();
        return CallAsync(HttpMethod.Delete, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /users/{userId}/meetings - list a single page of a user's meetings.</summary>
    public Task<ListMeetingsResult?> ListMeetingsAsync(
        string userId,
        string? type = null,
        int? pageSize = null,
        string? nextPageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (type != null) query["type"] = type;
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;

        return CallAsync<ListMeetingsResult>(HttpMethod.Get, $"/users/{Uri.EscapeDataString(userId)}/meetings", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>Walks every page of a user's meetings via ZoomPaging, yielding one MeetingSummary at a time.</summary>
    public IAsyncEnumerable<MeetingSummary> EnumerateMeetingsAsync(string userId, string? type = null, CancellationToken cancellationToken = default)
        => ZoomPaging.EnumerateAsync(
            (token, ct) => ListMeetingsAsync(userId, type, pageSize: 100, nextPageToken: token, cancellationToken: ct),
            result => result?.Meetings,
            result => result?.NextPageToken,
            cancellationToken);

    /// <summary>GET /users/{userId}/upcoming_meetings - list a user's upcoming meetings (not paginated by Zoom).</summary>
    public async Task<List<MeetingSummary>> ListUpcomingMeetingsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var result = await CallAsync<ListMeetingsResult>(HttpMethod.Get, $"/users/{Uri.EscapeDataString(userId)}/upcoming_meetings", cancellationToken: cancellationToken).ConfigureAwait(false);
        return result?.Meetings ?? new List<MeetingSummary>();
    }

    /// <summary>PUT /meetings/{meetingId}/status - end a meeting ("end") or recover it ("recover").</summary>
    public Task UpdateMeetingStatusAsync(string meetingId, string action, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Put, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/status", new { action }, cancellationToken: cancellationToken);

    /// <summary>GET /meetings/{meetingId}/invitation - get the meeting invitation text Zoom would email out.</summary>
    public Task<MeetingInvitation?> GetMeetingInvitationAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<MeetingInvitation>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/invitation", cancellationToken: cancellationToken);
}
