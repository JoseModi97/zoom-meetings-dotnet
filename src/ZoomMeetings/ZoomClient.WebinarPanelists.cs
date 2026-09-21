using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>GET /webinars/{webinarId}/panelists - list a webinar's panelists.</summary>
    public Task<ListWebinarPanelistsResult?> ListWebinarPanelistsAsync(string webinarId, CancellationToken cancellationToken = default)
        => CallAsync<ListWebinarPanelistsResult>(HttpMethod.Get, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/panelists", cancellationToken: cancellationToken);

    /// <summary>POST /webinars/{webinarId}/panelists - add one or more panelists.</summary>
    public Task<AddPanelistsResult?> AddWebinarPanelistsAsync(string webinarId, AddPanelistsRequest request, CancellationToken cancellationToken = default)
        => CallAsync<AddPanelistsResult>(HttpMethod.Post, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/panelists", request, cancellationToken: cancellationToken);

    /// <summary>DELETE /webinars/{webinarId}/panelists - remove all panelists.</summary>
    public Task RemoveAllWebinarPanelistsAsync(string webinarId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/panelists", cancellationToken: cancellationToken);

    /// <summary>DELETE /webinars/{webinarId}/panelists/{panelistId} - remove a single panelist.</summary>
    public Task RemoveWebinarPanelistAsync(string webinarId, string panelistId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/panelists/{Uri.EscapeDataString(panelistId)}", cancellationToken: cancellationToken);
}
