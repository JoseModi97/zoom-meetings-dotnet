using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>GET /report/meetings/{meetingId} - meeting detail report (only available after the meeting ends).</summary>
    public Task<MeetingReportDetail?> GetMeetingReportDetailAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<MeetingReportDetail>(HttpMethod.Get, $"/report/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}", cancellationToken: cancellationToken);

    /// <summary>GET /report/meetings/{meetingId}/participants - meeting participants report.</summary>
    public Task<ListMeetingReportParticipantsResult?> GetMeetingReportParticipantsAsync(
        string meetingId,
        int? pageSize = null,
        string? nextPageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;

        return CallAsync<ListMeetingReportParticipantsResult>(HttpMethod.Get, $"/report/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/participants", query: query, cancellationToken: cancellationToken);
    }
}
