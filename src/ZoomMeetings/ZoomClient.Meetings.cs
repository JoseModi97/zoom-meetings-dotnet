using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>POST /users/{userId}/meetings - create a scheduled/instant/recurring meeting for a user.</summary>
    public Task<Meeting?> CreateMeetingAsync(string userId, CreateMeetingRequest request, CancellationToken cancellationToken = default)
        => CallAsync<Meeting>(HttpMethod.Post, $"/users/{Uri.EscapeDataString(userId)}/meetings", request, cancellationToken: cancellationToken);

    /// <summary>GET /meetings/{meetingId} - get details for a single meeting.</summary>
    public Task<Meeting?> GetMeetingAsync(string meetingId, string? occurrenceId = null, CancellationToken cancellationToken = default)
    {
        var query = occurrenceId == null ? null : new Dictionary<string, string?> { ["occurrence_id"] = occurrenceId };
        return CallAsync<Meeting>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>PATCH /meetings/{meetingId} - update a meeting. Only set the fields you want to change on <paramref name="request"/>.</summary>
    public Task UpdateMeetingAsync(string meetingId, UpdateMeetingRequest request, string? occurrenceId = null, CancellationToken cancellationToken = default)
    {
        var query = occurrenceId == null ? null : new Dictionary<string, string?> { ["occurrence_id"] = occurrenceId };
        return CallAsync(HttpMethods.Patch, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}", request, query, cancellationToken);
    }

    /// <summary>DELETE /meetings/{meetingId} - delete/cancel a meeting.</summary>
    public Task DeleteMeetingAsync(string meetingId, string? occurrenceId = null, bool? scheduleForReminder = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (occurrenceId != null) query["occurrence_id"] = occurrenceId;
        if (scheduleForReminder != null) query["schedule_for_reminder"] = scheduleForReminder.Value.ToString().ToLowerInvariant();
        return CallAsync(HttpMethod.Delete, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /users/{userId}/meetings - list a single page of a user's meetings.</summary>
    public Task<ListMeetingsResult?> ListMeetingsAsync(
        string userId,
        string? type = null,
        int? pageSize = null,
        string? nextPageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (type != null) query["type"] = type;
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;

        return CallAsync<ListMeetingsResult>(HttpMethod.Get, $"/users/{Uri.EscapeDataString(userId)}/meetings", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>Walks every page of a user's meetings via ZoomPaging, yielding one MeetingSummary at a time.</summary>
    public IAsyncEnumerable<MeetingSummary> EnumerateMeetingsAsync(string userId, string? type = null, CancellationToken cancellationToken = default)
        => ZoomPaging.EnumerateAsync(
            (token, ct) => ListMeetingsAsync(userId, type, pageSize: 100, nextPageToken: token, cancellationToken: ct),
            result => result?.Meetings,
            result => result?.NextPageToken,
            cancellationToken);

    /// <summary>GET /users/{userId}/upcoming_meetings - list a user's upcoming meetings (not paginated by Zoom).</summary>
    public async Task<List<MeetingSummary>> ListUpcomingMeetingsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var result = await CallAsync<ListMeetingsResult>(HttpMethod.Get, $"/users/{Uri.EscapeDataString(userId)}/upcoming_meetings", cancellationToken: cancellationToken).ConfigureAwait(false);
        return result?.Meetings ?? new List<MeetingSummary>();
    }

    /// <summary>PUT /meetings/{meetingId}/status - end a meeting ("end") or recover it ("recover").</summary>
    public Task UpdateMeetingStatusAsync(string meetingId, string action, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Put, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/status", new { action }, cancellationToken: cancellationToken);

    /// <summary>GET /meetings/{meetingId}/invitation - get the meeting invitation text Zoom would email out.</summary>
    public Task<MeetingInvitation?> GetMeetingInvitationAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<MeetingInvitation>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/invitation", cancellationToken: cancellationToken);

    /// <summary>POST /meetings/{meetingId}/invite_links - create shareable, pre-filled invite links for named attendees.</summary>
    public Task<CreateInviteLinksResult?> CreateMeetingInviteLinksAsync(string meetingId, CreateInviteLinksRequest request, CancellationToken cancellationToken = default)
        => CreateInviteLinksForResourceAsync("meetings", meetingId, request, cancellationToken);

    /// <summary>GET /meetings/{meetingId}/jointoken/live_streaming - get a meeting's live-streaming join token.</summary>
    public Task<JoinTokenResult?> GetMeetingLiveStreamingJoinTokenAsync(string meetingId, CancellationToken cancellationToken = default)
        => GetJoinTokenForResourceAsync("meetings", meetingId, "live_streaming", cancellationToken);

    /// <summary>GET /meetings/{meetingId}/jointoken/local_archiving - get a meeting's local-archiving token.</summary>
    public Task<JoinTokenResult?> GetMeetingLocalArchivingTokenAsync(string meetingId, CancellationToken cancellationToken = default)
        => GetJoinTokenForResourceAsync("meetings", meetingId, "local_archiving", cancellationToken);

    /// <summary>GET /meetings/{meetingId}/jointoken/local_recording - get a meeting's local-recording join token.</summary>
    public Task<JoinTokenResult?> GetMeetingLocalRecordingJoinTokenAsync(string meetingId, bool? bypassWaitingRoom = null, CancellationToken cancellationToken = default)
    {
        var query = bypassWaitingRoom == null ? null : new Dictionary<string, string?> { ["bypass_waiting_room"] = bypassWaitingRoom.Value.ToString().ToLowerInvariant() };
        return CallAsync<JoinTokenResult>(HttpMethod.Get, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/jointoken/local_recording", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /meetings/{meetingId}/livestream - get a meeting's live-stream configuration.</summary>
    public Task<LiveStreamDetails?> GetMeetingLiveStreamDetailsAsync(string meetingId, CancellationToken cancellationToken = default)
        => GetLiveStreamDetailsForResourceAsync("meetings", meetingId, cancellationToken);

    /// <summary>PATCH /meetings/{meetingId}/livestream - configure a meeting's live-stream destination.</summary>
    public Task UpdateMeetingLiveStreamAsync(string meetingId, UpdateLiveStreamRequest request, CancellationToken cancellationToken = default)
        => UpdateLiveStreamForResourceAsync("meetings", meetingId, request, cancellationToken);

    /// <summary>PATCH /meetings/{meetingId}/livestream/status - start or stop a meeting's live stream.</summary>
    public Task UpdateMeetingLiveStreamStatusAsync(string meetingId, UpdateLiveStreamStatusRequest request, CancellationToken cancellationToken = default)
        => UpdateLiveStreamStatusForResourceAsync("meetings", meetingId, request, cancellationToken);

    /// <summary>POST /meetings/{meetingId}/open_apps - add a meeting app (Zoom App) to a meeting.</summary>
    public Task<MeetingAppResult?> AddMeetingAppAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<MeetingAppResult>(HttpMethod.Post, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/open_apps", cancellationToken: cancellationToken);

    /// <summary>DELETE /meetings/{meetingId}/open_apps - remove a meeting app (Zoom App) from a meeting.</summary>
    public Task DeleteMeetingAppAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/open_apps", cancellationToken: cancellationToken);

    /// <summary>POST /meetings/{meetingId}/sip_dialing - get a meeting's SIP URI, optionally with a passcode baked in.</summary>
    public Task<SipDialingResult?> GetMeetingSipDialingAsync(string meetingId, string? passcode = null, CancellationToken cancellationToken = default)
        => GetSipDialingForResourceAsync("meetings", meetingId, passcode, cancellationToken);

    /// <summary>GET /meetings/{meetingId}/survey - get a meeting's post-meeting survey configuration.</summary>
    public Task<MeetingSurvey?> GetMeetingSurveyAsync(string meetingId, CancellationToken cancellationToken = default)
        => GetSurveyForResourceAsync("meetings", meetingId, cancellationToken);

    /// <summary>PATCH /meetings/{meetingId}/survey - update a meeting's post-meeting survey configuration.</summary>
    public Task UpdateMeetingSurveyAsync(string meetingId, MeetingSurvey survey, CancellationToken cancellationToken = default)
        => UpdateSurveyForResourceAsync("meetings", meetingId, survey, cancellationToken);

    /// <summary>DELETE /meetings/{meetingId}/survey - delete a meeting's post-meeting survey configuration.</summary>
    public Task DeleteMeetingSurveyAsync(string meetingId, CancellationToken cancellationToken = default)
        => DeleteSurveyForResourceAsync("meetings", meetingId, cancellationToken);

    /// <summary>GET /meetings/{meetingId}/token - get a meeting's token (used by the Zoom Web SDK, among others).</summary>
    public Task<TokenResult?> GetMeetingTokenAsync(string meetingId, string? type = null, CancellationToken cancellationToken = default)
        => GetTokenForResourceAsync("meetings", meetingId, type, cancellationToken);

    /// <summary>GET /past_meetings/{meetingId} - get details for a meeting that has already occurred.</summary>
    public Task<PastMeetingDetail?> GetPastMeetingDetailsAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<PastMeetingDetail>(HttpMethod.Get, $"/past_meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}", cancellationToken: cancellationToken);

    /// <summary>GET /past_meetings/{meetingId}/instances - list the past instances of a recurring meeting.</summary>
    public async Task<List<PastInstance>> ListPastMeetingInstancesAsync(string meetingId, CancellationToken cancellationToken = default)
    {
        var result = await CallAsync<ListPastInstancesResult>(HttpMethod.Get, $"/past_meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/instances", cancellationToken: cancellationToken).ConfigureAwait(false);
        return result?.Meetings ?? new List<PastInstance>();
    }

    /// <summary>GET /past_meetings/{meetingId}/participants - list a single page of a past meeting's participants.</summary>
    public Task<ListPastParticipantsResult?> ListPastMeetingParticipantsAsync(string meetingId, int? pageSize = null, string? nextPageToken = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        return CallAsync<ListPastParticipantsResult>(HttpMethod.Get, $"/past_meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/participants", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>Walks every page of a past meeting's participants via ZoomPaging.</summary>
    public IAsyncEnumerable<PastParticipant> EnumeratePastMeetingParticipantsAsync(string meetingId, CancellationToken cancellationToken = default)
        => ZoomPaging.EnumerateAsync(
            (token, ct) => ListPastMeetingParticipantsAsync(meetingId, pageSize: 100, nextPageToken: token, cancellationToken: ct),
            result => result?.Participants,
            result => result?.NextPageToken,
            cancellationToken);

    /// <summary>GET /past_meetings/{meetingId}/qa - list a past meeting's Q&amp;A.</summary>
    public Task<QaResult?> ListPastMeetingQaAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<QaResult>(HttpMethod.Get, $"/past_meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/qa", cancellationToken: cancellationToken);

    /// <summary>GET /users/{userId}/pac - list a user's Personal Audio Conference (PAC) accounts.</summary>
    public async Task<List<PacAccount>> ListUserPacAccountsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var result = await CallAsync<ListPacAccountsResult>(HttpMethod.Get, $"/users/{Uri.EscapeDataString(userId)}/pac", cancellationToken: cancellationToken).ConfigureAwait(false);
        return result?.PacAccounts ?? new List<PacAccount>();
    }
}
