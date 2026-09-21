using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

/// <summary>
/// Shared implementations for operations whose request/response shapes are identical across meetings
/// and webinars in Zoom's spec - only the "meetings"/"webinars" path segment differs (the same pattern
/// already used for Registrants in ZoomClient.Registrants.cs). ZoomClient.Meetings.cs and
/// ZoomClient.Webinars.cs expose thin public wrappers around these.
/// </summary>
public sealed partial class ZoomClient
{
    private Task<CreateInviteLinksResult?> CreateInviteLinksForResourceAsync(string resource, string resourceId, CreateInviteLinksRequest request, CancellationToken cancellationToken)
        => CallAsync<CreateInviteLinksResult>(HttpMethod.Post, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/invite_links", request, cancellationToken: cancellationToken);

    private Task<JoinTokenResult?> GetJoinTokenForResourceAsync(string resource, string resourceId, string tokenType, CancellationToken cancellationToken)
        => CallAsync<JoinTokenResult>(HttpMethod.Get, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/jointoken/{tokenType}", cancellationToken: cancellationToken);

    private Task<LiveStreamDetails?> GetLiveStreamDetailsForResourceAsync(string resource, string resourceId, CancellationToken cancellationToken)
        => CallAsync<LiveStreamDetails>(HttpMethod.Get, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/livestream", cancellationToken: cancellationToken);

    private Task UpdateLiveStreamForResourceAsync(string resource, string resourceId, UpdateLiveStreamRequest request, CancellationToken cancellationToken)
        => CallAsync(HttpMethods.Patch, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/livestream", request, cancellationToken: cancellationToken);

    private Task UpdateLiveStreamStatusForResourceAsync(string resource, string resourceId, UpdateLiveStreamStatusRequest request, CancellationToken cancellationToken)
        => CallAsync(HttpMethods.Patch, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/livestream/status", request, cancellationToken: cancellationToken);

    private Task<SipDialingResult?> GetSipDialingForResourceAsync(string resource, string resourceId, string? passcode, CancellationToken cancellationToken)
        => CallAsync<SipDialingResult>(HttpMethod.Post, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/sip_dialing", new { passcode }, cancellationToken: cancellationToken);

    private Task<MeetingSurvey?> GetSurveyForResourceAsync(string resource, string resourceId, CancellationToken cancellationToken)
        => CallAsync<MeetingSurvey>(HttpMethod.Get, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/survey", cancellationToken: cancellationToken);

    private Task UpdateSurveyForResourceAsync(string resource, string resourceId, MeetingSurvey survey, CancellationToken cancellationToken)
        => CallAsync(HttpMethods.Patch, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/survey", survey, cancellationToken: cancellationToken);

    private Task DeleteSurveyForResourceAsync(string resource, string resourceId, CancellationToken cancellationToken)
        => CallAsync(HttpMethod.Delete, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/survey", cancellationToken: cancellationToken);

    private Task<TokenResult?> GetTokenForResourceAsync(string resource, string resourceId, string? type, CancellationToken cancellationToken)
    {
        var query = type == null ? null : new Dictionary<string, string?> { ["type"] = type };
        return CallAsync<TokenResult>(HttpMethod.Get, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/token", query: query, cancellationToken: cancellationToken);
    }

    private Task<RegistrationQuestions?> GetRegistrationQuestionsForResourceAsync(string resource, string resourceId, CancellationToken cancellationToken)
        => CallAsync<RegistrationQuestions>(HttpMethod.Get, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/registrants/questions", cancellationToken: cancellationToken);

    private Task UpdateRegistrationQuestionsForResourceAsync(string resource, string resourceId, RegistrationQuestions questions, CancellationToken cancellationToken)
        => CallAsync(HttpMethods.Patch, $"/{resource}/{ZoomIdEncoding.EncodePathSegment(resourceId)}/registrants/questions", questions, cancellationToken: cancellationToken);
}
