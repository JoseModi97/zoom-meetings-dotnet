using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>GET /meetings/{meetingId}/registrants - list a single page of a meeting's registrants.</summary>
    public Task<ListRegistrantsResult?> ListRegistrantsAsync(
        string meetingId,
        string? status = null,
        int? pageSize = null,
        string? nextPageToken = null,
        CancellationToken cancellationToken = default)
        => ListRegistrantsForResourceAsync("meetings", meetingId, status, pageSize, nextPageToken, cancellationToken);

    /// <summary>Walks every page of a meeting's registrants via ZoomPaging.</summary>
    public IAsyncEnumerable<Registrant> EnumerateRegistrantsAsync(string meetingId, string? status = null, CancellationToken cancellationToken = default)
        => ZoomPaging.EnumerateAsync(
            (token, ct) => ListRegistrantsAsync(meetingId, status, pageSize: 100, nextPageToken: token, cancellationToken: ct),
            result => result?.Registrants,
            result => result?.NextPageToken,
            cancellationToken);

    /// <summary>POST /meetings/{meetingId}/registrants - add a registrant to a meeting.</summary>
    public Task<AddRegistrantResult?> AddRegistrantAsync(string meetingId, AddRegistrantRequest request, CancellationToken cancellationToken = default)
        => AddRegistrantForResourceAsync("meetings", meetingId, request, cancellationToken);

    /// <summary>GET /meetings/{meetingId}/registrants/{registrantId} - get a single registrant.</summary>
    public Task<Registrant?> GetRegistrantAsync(string meetingId, string registrantId, CancellationToken cancellationToken = default)
        => CallAsync<Registrant>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/registrants/{Uri.EscapeDataString(registrantId)}", cancellationToken: cancellationToken);

    /// <summary>DELETE /meetings/{meetingId}/registrants/{registrantId} - remove a registrant.</summary>
    public Task DeleteRegistrantAsync(string meetingId, string registrantId, string? occurrenceId = null, CancellationToken cancellationToken = default)
    {
        var query = occurrenceId == null ? null : new Dictionary<string, string?> { ["occurrence_id"] = occurrenceId };
        return CallAsync(HttpMethod.Delete, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/registrants/{Uri.EscapeDataString(registrantId)}", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>PUT /meetings/{meetingId}/registrants/status - approve, deny, or cancel one or more registrants.</summary>
    public Task UpdateRegistrantStatusAsync(string meetingId, string action, IEnumerable<string> registrantIds, CancellationToken cancellationToken = default)
        => UpdateRegistrantStatusForResourceAsync("meetings", meetingId, action, registrantIds, cancellationToken);

    // Shared implementation used by both meetings and webinars (ZoomClient.Webinars.cs) - the
    // request/response shapes are identical, only the resource segment in the path differs.
    private Task<ListRegistrantsResult?> ListRegistrantsForResourceAsync(
        string resource, string resourceId, string? status, int? pageSize, string? nextPageToken, CancellationToken cancellationToken)
    {
        var query = new Dictionary<string, string?>();
        if (status != null) query["status"] = status;
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;

        return CallAsync<ListRegistrantsResult>(HttpMethod.Get, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/registrants", query: query, cancellationToken: cancellationToken);
    }

    private Task<AddRegistrantResult?> AddRegistrantForResourceAsync(string resource, string resourceId, AddRegistrantRequest request, CancellationToken cancellationToken)
        => CallAsync<AddRegistrantResult>(HttpMethod.Post, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/registrants", request, cancellationToken: cancellationToken);

    private Task UpdateRegistrantStatusForResourceAsync(string resource, string resourceId, string action, IEnumerable<string> registrantIds, CancellationToken cancellationToken)
    {
        var body = new
        {
            action,
            registrants = registrantIds.Select(id => new { id }).ToList(),
        };
        return CallAsync(HttpMethod.Put, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/registrants/status", body, cancellationToken: cancellationToken);
    }
}
