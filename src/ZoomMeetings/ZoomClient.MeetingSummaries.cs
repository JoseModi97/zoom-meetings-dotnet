using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>GET /meetings/{meetingId}/meeting_summary - get an AI-generated meeting/webinar summary.</summary>
    public Task<MeetingSummaryDetail?> GetMeetingSummaryAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<MeetingSummaryDetail>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/meeting_summary", cancellationToken: cancellationToken);

    /// <summary>DELETE /meetings/{meetingId}/meeting_summary - delete a meeting/webinar summary.</summary>
    public Task DeleteMeetingSummaryAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/meeting_summary", cancellationToken: cancellationToken);

    /// <summary>GET /meetings/meeting_summaries - list an account's meeting/webinar summaries.</summary>
    public Task<ListMeetingSummariesResult?> ListAccountMeetingSummariesAsync(
        int? pageSize = null,
        string? nextPageToken = null,
        DateTimeOffset? from = null,
        DateTimeOffset? to = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (from != null) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to != null) query["to"] = to.Value.ToString("yyyy-MM-dd");

        return CallAsync<ListMeetingSummariesResult>(HttpMethod.Get, "/meetings/meeting_summaries", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /users/{userId}/meeting_summaries - list a user's meeting or webinar summaries.</summary>
    public Task<ListMeetingSummariesResult?> ListUserMeetingSummariesAsync(
        string userId,
        int? pageSize = null,
        string? nextPageToken = null,
        DateTimeOffset? from = null,
        DateTimeOffset? to = null,
        string? timeFilterField = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (from != null) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to != null) query["to"] = to.Value.ToString("yyyy-MM-dd");
        if (timeFilterField != null) query["time_filter_field"] = timeFilterField;

        return CallAsync<ListMeetingSummariesResult>(HttpMethod.Get, $"/users/{ZoomIdEncoding.EncodePathSegment(userId)}/meeting_summaries", query: query, cancellationToken: cancellationToken);
    }
}
