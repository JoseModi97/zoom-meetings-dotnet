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

    /// <summary>GET /webinars/{webinarId}/registrants/questions - get a webinar's registration questions.</summary>
    public Task<RegistrationQuestions?> GetWebinarRegistrationQuestionsAsync(string webinarId, CancellationToken cancellationToken = default)
        => GetRegistrationQuestionsForResourceAsync("webinars", webinarId, cancellationToken);

    /// <summary>PATCH /webinars/{webinarId}/registrants/questions - update a webinar's registration questions.</summary>
    public Task UpdateWebinarRegistrationQuestionsAsync(string webinarId, RegistrationQuestions questions, CancellationToken cancellationToken = default)
        => UpdateRegistrationQuestionsForResourceAsync("webinars", webinarId, questions, cancellationToken);

    /// <summary>POST /webinars/{webinarId}/batch_registrants - register many attendees in one call.</summary>
    public Task<AddBatchRegistrantsResult?> AddBatchWebinarRegistrantsAsync(string webinarId, AddBatchRegistrantsRequest request, CancellationToken cancellationToken = default)
        => CallAsync<AddBatchRegistrantsResult>(HttpMethod.Post, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/batch_registrants", request, cancellationToken: cancellationToken);

    /// <summary>POST /webinars/{webinarId}/invite_links - create shareable, pre-filled invite links for named attendees.</summary>
    public Task<CreateInviteLinksResult?> CreateWebinarInviteLinksAsync(string webinarId, CreateInviteLinksRequest request, CancellationToken cancellationToken = default)
        => CreateInviteLinksForResourceAsync("webinars", webinarId, request, cancellationToken);

    /// <summary>GET /webinars/{webinarId}/jointoken/live_streaming - get a webinar's live-streaming join token.</summary>
    public Task<JoinTokenResult?> GetWebinarLiveStreamingJoinTokenAsync(string webinarId, CancellationToken cancellationToken = default)
        => GetJoinTokenForResourceAsync("webinars", webinarId, "live_streaming", cancellationToken);

    /// <summary>GET /webinars/{webinarId}/jointoken/local_archiving - get a webinar's local-archiving token.</summary>
    public Task<JoinTokenResult?> GetWebinarLocalArchivingTokenAsync(string webinarId, CancellationToken cancellationToken = default)
        => GetJoinTokenForResourceAsync("webinars", webinarId, "local_archiving", cancellationToken);

    /// <summary>GET /webinars/{webinarId}/jointoken/local_recording - get a webinar's local-recording join token.</summary>
    public Task<JoinTokenResult?> GetWebinarLocalRecordingJoinTokenAsync(string webinarId, CancellationToken cancellationToken = default)
        => GetJoinTokenForResourceAsync("webinars", webinarId, "local_recording", cancellationToken);

    /// <summary>GET /webinars/{webinarId}/livestream - get a webinar's live-stream configuration.</summary>
    public Task<LiveStreamDetails?> GetWebinarLiveStreamDetailsAsync(string webinarId, CancellationToken cancellationToken = default)
        => GetLiveStreamDetailsForResourceAsync("webinars", webinarId, cancellationToken);

    /// <summary>PATCH /webinars/{webinarId}/livestream - configure a webinar's live-stream destination.</summary>
    public Task UpdateWebinarLiveStreamAsync(string webinarId, UpdateLiveStreamRequest request, CancellationToken cancellationToken = default)
        => UpdateLiveStreamForResourceAsync("webinars", webinarId, request, cancellationToken);

    /// <summary>PATCH /webinars/{webinarId}/livestream/status - start or stop a webinar's live stream.</summary>
    public Task UpdateWebinarLiveStreamStatusAsync(string webinarId, UpdateLiveStreamStatusRequest request, CancellationToken cancellationToken = default)
        => UpdateLiveStreamStatusForResourceAsync("webinars", webinarId, request, cancellationToken);

    /// <summary>POST /webinars/{webinarId}/sip_dialing - get a webinar's SIP URI, optionally with a passcode baked in.</summary>
    public Task<SipDialingResult?> GetWebinarSipDialingAsync(string webinarId, string? passcode = null, CancellationToken cancellationToken = default)
        => GetSipDialingForResourceAsync("webinars", webinarId, passcode, cancellationToken);

    /// <summary>GET /webinars/{webinarId}/survey - get a webinar's post-event survey configuration.</summary>
    public Task<MeetingSurvey?> GetWebinarSurveyAsync(string webinarId, CancellationToken cancellationToken = default)
        => GetSurveyForResourceAsync("webinars", webinarId, cancellationToken);

    /// <summary>PATCH /webinars/{webinarId}/survey - update a webinar's post-event survey configuration.</summary>
    public Task UpdateWebinarSurveyAsync(string webinarId, MeetingSurvey survey, CancellationToken cancellationToken = default)
        => UpdateSurveyForResourceAsync("webinars", webinarId, survey, cancellationToken);

    /// <summary>DELETE /webinars/{webinarId}/survey - delete a webinar's post-event survey configuration.</summary>
    public Task DeleteWebinarSurveyAsync(string webinarId, CancellationToken cancellationToken = default)
        => DeleteSurveyForResourceAsync("webinars", webinarId, cancellationToken);

    /// <summary>GET /webinars/{webinarId}/token - get a webinar's token (used by the Zoom Web SDK, among others).</summary>
    public Task<TokenResult?> GetWebinarTokenAsync(string webinarId, string? type = null, CancellationToken cancellationToken = default)
        => GetTokenForResourceAsync("webinars", webinarId, type, cancellationToken);

    /// <summary>PUT /webinars/{webinarId}/status - end a webinar.</summary>
    public Task UpdateWebinarStatusAsync(string webinarId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Put, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/status", new { action = "end" }, cancellationToken: cancellationToken);

    /// <summary>GET /webinars/{webinarId}/tracking_sources - list a webinar's tracking sources and their registration/visitor counts.</summary>
    public async Task<List<TrackingSource>> GetWebinarTrackingSourcesAsync(string webinarId, CancellationToken cancellationToken = default)
    {
        var result = await CallAsync<ListTrackingSourcesResult>(HttpMethod.Get, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/tracking_sources", cancellationToken: cancellationToken).ConfigureAwait(false);
        return result?.TrackingSources ?? new List<TrackingSource>();
    }

    /// <summary>DELETE /live_webinars/{webinarId}/chat/messages/{messageId} - delete a message from a live webinar's chat.</summary>
    public Task DeleteLiveWebinarChatMessageAsync(string webinarId, string messageId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/live_webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/chat/messages/{Uri.EscapeDataString(messageId)}", cancellationToken: cancellationToken);

    /// <summary>GET /users/{userId}/webinar_templates - list a user's webinar templates.</summary>
    public Task<ListWebinarTemplatesResult?> ListWebinarTemplatesAsync(string userId, CancellationToken cancellationToken = default)
        => CallAsync<ListWebinarTemplatesResult>(HttpMethod.Get, $"/users/{Uri.EscapeDataString(userId)}/webinar_templates", cancellationToken: cancellationToken);

    /// <summary>POST /users/{userId}/webinar_templates - save an existing webinar as a reusable template.</summary>
    public Task<WebinarTemplate?> CreateWebinarTemplateAsync(string userId, CreateWebinarTemplateRequest request, CancellationToken cancellationToken = default)
        => CallAsync<WebinarTemplate>(HttpMethod.Post, $"/users/{Uri.EscapeDataString(userId)}/webinar_templates", request, cancellationToken: cancellationToken);

    /// <summary>GET /past_webinars/{webinarId}/absentees - list a single page of a past webinar's registrants who did not attend.</summary>
    public Task<ListRegistrantsResult?> ListPastWebinarAbsenteesAsync(string webinarId, string? occurrenceId = null, int? pageSize = null, string? nextPageToken = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (occurrenceId != null) query["occurrence_id"] = occurrenceId;
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        return CallAsync<ListRegistrantsResult>(HttpMethod.Get, $"/past_webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/absentees", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /past_webinars/{webinarId}/instances - list the past instances of a recurring webinar.</summary>
    public async Task<List<PastInstance>> ListPastWebinarInstancesAsync(string webinarId, CancellationToken cancellationToken = default)
    {
        var result = await CallAsync<ListPastInstancesResult>(HttpMethod.Get, $"/past_webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/instances", cancellationToken: cancellationToken).ConfigureAwait(false);
        return result?.Webinars ?? new List<PastInstance>();
    }

    /// <summary>GET /past_webinars/{webinarId}/participants - list a single page of a past webinar's participants.</summary>
    public Task<ListPastParticipantsResult?> ListPastWebinarParticipantsAsync(string webinarId, int? pageSize = null, string? nextPageToken = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        return CallAsync<ListPastParticipantsResult>(HttpMethod.Get, $"/past_webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/participants", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /past_webinars/{webinarId}/polls - list a past webinar's poll results.</summary>
    public Task<QaResult?> ListPastWebinarPollResultsAsync(string webinarId, CancellationToken cancellationToken = default)
        => CallAsync<QaResult>(HttpMethod.Get, $"/past_webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/polls", cancellationToken: cancellationToken);

    /// <summary>GET /past_webinars/{webinarId}/qa - list a past webinar's Q&amp;A.</summary>
    public Task<QaResult?> ListPastWebinarQaAsync(string webinarId, CancellationToken cancellationToken = default)
        => CallAsync<QaResult>(HttpMethod.Get, $"/past_webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/qa", cancellationToken: cancellationToken);
}
