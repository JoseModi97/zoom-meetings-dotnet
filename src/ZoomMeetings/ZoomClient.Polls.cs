using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>GET /meetings/{meetingId}/polls - list a meeting's polls.</summary>
    public Task<ListPollsResult?> ListPollsAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<ListPollsResult>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/polls", cancellationToken: cancellationToken);

    /// <summary>POST /meetings/{meetingId}/polls - create a poll.</summary>
    public Task<Poll?> CreatePollAsync(string meetingId, Poll poll, CancellationToken cancellationToken = default)
        => CallAsync<Poll>(HttpMethod.Post, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/polls", poll, cancellationToken: cancellationToken);

    /// <summary>GET /meetings/{meetingId}/polls/{pollId} - get a single poll.</summary>
    public Task<Poll?> GetPollAsync(string meetingId, string pollId, CancellationToken cancellationToken = default)
        => CallAsync<Poll>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/polls/{Uri.EscapeDataString(pollId)}", cancellationToken: cancellationToken);

    /// <summary>PUT /meetings/{meetingId}/polls/{pollId} - update a poll.</summary>
    public Task UpdatePollAsync(string meetingId, string pollId, Poll poll, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Put, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/polls/{Uri.EscapeDataString(pollId)}", poll, cancellationToken: cancellationToken);

    /// <summary>DELETE /meetings/{meetingId}/polls/{pollId} - delete a poll.</summary>
    public Task DeletePollAsync(string meetingId, string pollId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/polls/{Uri.EscapeDataString(pollId)}", cancellationToken: cancellationToken);
}
