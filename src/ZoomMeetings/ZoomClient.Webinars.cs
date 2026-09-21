using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>POST /users/{userId}/webinars - create a webinar.</summary>
    public Task<Webinar?> CreateWebinarAsync(string userId, CreateWebinarRequest request, CancellationToken cancellationToken = default)
        => CallAsync<Webinar>(HttpMethod.Post, $"/users/{Uri.EscapeDataString(userId)}/webinars", request, cancellationToken: cancellationToken);

    /// <summary>GET /webinars/{webinarId} - get details for a single webinar.</summary>
    public Task<Webinar?> GetWebinarAsync(string webinarId, CancellationToken cancellationToken = default)
        => CallAsync<Webinar>(HttpMethod.Get, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}", cancellationToken: cancellationToken);

    /// <summary>PATCH /webinars/{webinarId} - update a webinar. Only set the fields you want to change on <paramref name="request"/>.</summary>
    public Task UpdateWebinarAsync(string webinarId, UpdateWebinarRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}", request, cancellationToken: cancellationToken);

    /// <summary>DELETE /webinars/{webinarId} - delete/cancel a webinar.</summary>
    public Task DeleteWebinarAsync(string webinarId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}", cancellationToken: cancellationToken);

    /// <summary>GET /users/{userId}/webinars - list a single page of a user's webinars.</summary>
    public Task<ListWebinarsResult?> ListWebinarsAsync(
        string userId,
        int? pageSize = null,
        string? nextPageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;

        return CallAsync<ListWebinarsResult>(HttpMethod.Get, $"/users/{Uri.EscapeDataString(userId)}/webinars", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>Walks every page of a user's webinars via ZoomPaging.</summary>
    public IAsyncEnumerable<WebinarSummary> EnumerateWebinarsAsync(string userId, CancellationToken cancellationToken = default)
        => ZoomPaging.EnumerateAsync(
            (token, ct) => ListWebinarsAsync(userId, pageSize: 100, nextPageToken: token, cancellationToken: ct),
            result => result?.Webinars,
            result => result?.NextPageToken,
            cancellationToken);

    // Webinar registrants: reuse the same models and the shared *ForResourceAsync helpers in
    // ZoomClient.Registrants.cs - the shapes match closely enough across meetings and webinars
    // that duplicating them wasn't worth it, only the "meetings"/"webinars" path segment differs.

    /// <summary>GET /webinars/{webinarId}/registrants - list a single page of a webinar's registrants.</summary>
    public Task<ListRegistrantsResult?> ListWebinarRegistrantsAsync(
        string webinarId,
        string? status = null,
        int? pageSize = null,
        string? nextPageToken = null,
        CancellationToken cancellationToken = default)
        => ListRegistrantsForResourceAsync("webinars", webinarId, status, pageSize, nextPageToken, cancellationToken);

    /// <summary>Walks every page of a webinar's registrants via ZoomPaging.</summary>
    public IAsyncEnumerable<Registrant> EnumerateWebinarRegistrantsAsync(string webinarId, string? status = null, CancellationToken cancellationToken = default)
        => ZoomPaging.EnumerateAsync(
            (token, ct) => ListWebinarRegistrantsAsync(webinarId, status, pageSize: 100, nextPageToken: token, cancellationToken: ct),
            result => result?.Registrants,
            result => result?.NextPageToken,
            cancellationToken);

    /// <summary>POST /webinars/{webinarId}/registrants - add a registrant to a webinar.</summary>
    public Task<AddRegistrantResult?> AddWebinarRegistrantAsync(string webinarId, AddRegistrantRequest request, CancellationToken cancellationToken = default)
        => AddRegistrantForResourceAsync("webinars", webinarId, request, cancellationToken);

    /// <summary>GET /webinars/{webinarId}/registrants/{registrantId} - get a single webinar registrant.</summary>
    public Task<Registrant?> GetWebinarRegistrantAsync(string webinarId, string registrantId, CancellationToken cancellationToken = default)
        => CallAsync<Registrant>(HttpMethod.Get, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/registrants/{Uri.EscapeDataString(registrantId)}", cancellationToken: cancellationToken);

    /// <summary>DELETE /webinars/{webinarId}/registrants/{registrantId} - remove a webinar registrant.</summary>
    public Task DeleteWebinarRegistrantAsync(string webinarId, string registrantId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/registrants/{Uri.EscapeDataString(registrantId)}", cancellationToken: cancellationToken);

    /// <summary>PUT /webinars/{webinarId}/registrants/status - approve, deny, or cancel one or more webinar registrants.</summary>
    public Task UpdateWebinarRegistrantStatusAsync(string webinarId, string action, IEnumerable<string> registrantIds, CancellationToken cancellationToken = default)
        => UpdateRegistrantStatusForResourceAsync("webinars", webinarId, action, registrantIds, cancellationToken);
}
