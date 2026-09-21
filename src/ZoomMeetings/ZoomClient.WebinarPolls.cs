using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>GET /webinars/{webinarId}/polls - list a webinar's polls.</summary>
    public Task<ListPollsResult?> ListWebinarPollsAsync(string webinarId, bool? anonymous = null, CancellationToken cancellationToken = default)
    {
        var query = anonymous == null ? null : new Dictionary<string, string?> { ["anonymous"] = anonymous.Value.ToString().ToLowerInvariant() };
        return CallAsync<ListPollsResult>(HttpMethod.Get, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/polls", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>POST /webinars/{webinarId}/polls - create a poll.</summary>
    public Task<Poll?> CreateWebinarPollAsync(string webinarId, Poll poll, CancellationToken cancellationToken = default)
        => CallAsync<Poll>(HttpMethod.Post, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/polls", poll, cancellationToken: cancellationToken);

    /// <summary>GET /webinars/{webinarId}/polls/{pollId} - get a single poll.</summary>
    public Task<Poll?> GetWebinarPollAsync(string webinarId, string pollId, CancellationToken cancellationToken = default)
        => CallAsync<Poll>(HttpMethod.Get, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/polls/{Uri.EscapeDataString(pollId)}", cancellationToken: cancellationToken);

    /// <summary>PUT /webinars/{webinarId}/polls/{pollId} - update a poll.</summary>
    public Task UpdateWebinarPollAsync(string webinarId, string pollId, Poll poll, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Put, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/polls/{Uri.EscapeDataString(pollId)}", poll, cancellationToken: cancellationToken);

    /// <summary>DELETE /webinars/{webinarId}/polls/{pollId} - delete a poll.</summary>
    public Task DeleteWebinarPollAsync(string webinarId, string pollId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/polls/{Uri.EscapeDataString(pollId)}", cancellationToken: cancellationToken);
}
