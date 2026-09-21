using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>GET /tracking_fields - list tracking fields.</summary>
    public Task<ListTrackingFieldsResult?> ListTrackingFieldsAsync(CancellationToken cancellationToken = default)
        => CallAsync<ListTrackingFieldsResult>(HttpMethod.Get, "/tracking_fields", cancellationToken: cancellationToken);

    /// <summary>POST /tracking_fields - create a tracking field.</summary>
    public Task<TrackingField?> CreateTrackingFieldAsync(CreateTrackingFieldRequest request, CancellationToken cancellationToken = default)
        => CallAsync<TrackingField>(HttpMethod.Post, "/tracking_fields", request, cancellationToken: cancellationToken);

    /// <summary>GET /tracking_fields/{fieldId} - get a tracking field.</summary>
    public Task<TrackingField?> GetTrackingFieldAsync(string fieldId, CancellationToken cancellationToken = default)
        => CallAsync<TrackingField>(HttpMethod.Get, $"/tracking_fields/{Uri.EscapeDataString(fieldId)}", cancellationToken: cancellationToken);

    /// <summary>PATCH /tracking_fields/{fieldId} - update a tracking field.</summary>
    public Task UpdateTrackingFieldAsync(string fieldId, UpdateTrackingFieldRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/tracking_fields/{Uri.EscapeDataString(fieldId)}", request, cancellationToken: cancellationToken);

    /// <summary>DELETE /tracking_fields/{fieldId} - delete a tracking field.</summary>
    public Task DeleteTrackingFieldAsync(string fieldId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/tracking_fields/{Uri.EscapeDataString(fieldId)}", cancellationToken: cancellationToken);
}
