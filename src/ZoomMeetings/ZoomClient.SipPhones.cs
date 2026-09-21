using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>GET /sip_phones/phones - list SIP phones.</summary>
    public Task<ListSipPhonesResult?> ListSipPhonesAsync(
        string? searchKey = null,
        int? pageSize = null,
        string? nextPageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (searchKey != null) query["search_key"] = searchKey;
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;

        return CallAsync<ListSipPhonesResult>(HttpMethod.Get, "/sip_phones/phones", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>POST /sip_phones/phones - enable a SIP phone.</summary>
    public Task<SipPhone?> EnableSipPhoneAsync(EnableSipPhoneRequest request, CancellationToken cancellationToken = default)
        => CallAsync<SipPhone>(HttpMethod.Post, "/sip_phones/phones", request, cancellationToken: cancellationToken);

    /// <summary>PATCH /sip_phones/phones/{phoneId} - update a SIP phone.</summary>
    public Task UpdateSipPhoneAsync(string phoneId, UpdateSipPhoneRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/sip_phones/phones/{Uri.EscapeDataString(phoneId)}", request, cancellationToken: cancellationToken);

    /// <summary>DELETE /sip_phones/phones/{phoneId} - delete a SIP phone.</summary>
    public Task DeleteSipPhoneAsync(string phoneId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/sip_phones/phones/{Uri.EscapeDataString(phoneId)}", cancellationToken: cancellationToken);
}
