using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>GET /users/{userId}/meeting_templates - list meeting templates for a user.</summary>
    public Task<ListMeetingTemplatesResult?> ListMeetingTemplatesAsync(string userId, CancellationToken cancellationToken = default)
        => CallAsync<ListMeetingTemplatesResult>(HttpMethod.Get, $"/users/{ZoomIdEncoding.EncodePathSegment(userId)}/meeting_templates", cancellationToken: cancellationToken);

    /// <summary>POST /users/{userId}/meeting_templates - create a meeting template from an existing meeting.</summary>
    public Task<CreateMeetingTemplateResult?> CreateMeetingTemplateAsync(string userId, CreateMeetingTemplateRequest request, CancellationToken cancellationToken = default)
        => CallAsync<CreateMeetingTemplateResult>(HttpMethod.Post, $"/users/{ZoomIdEncoding.EncodePathSegment(userId)}/meeting_templates", request, cancellationToken: cancellationToken);
}
