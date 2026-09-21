using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>DELETE /live_meetings/{meetingId}/chat/messages/{messageId} - delete a live meeting chat message.</summary>
    public Task DeleteLiveMeetingChatMessageAsync(string meetingId, string messageId, string? fileIds = null, CancellationToken cancellationToken = default)
    {
        var query = fileIds == null ? null : new Dictionary<string, string?> { ["file_ids"] = fileIds };
        return CallAsync(HttpMethod.Delete, $"/live_meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/chat/messages/{Uri.EscapeDataString(messageId)}", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>PATCH /live_meetings/{meetingId}/chat/messages/{messageId} - update a live meeting chat message.</summary>
    public Task UpdateLiveMeetingChatMessageAsync(string meetingId, string messageId, string messageContent, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/live_meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/chat/messages/{Uri.EscapeDataString(messageId)}", new UpdateLiveChatMessageRequest { MessageContent = messageContent }, cancellationToken: cancellationToken);

    /// <summary>PATCH /live_meetings/{meetingId}/events - perform in-meeting controls.</summary>
    public Task InMeetingControlAsync(string meetingId, InMeetingControlRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/live_meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/events", request, cancellationToken: cancellationToken);

    /// <summary>PATCH /live_meetings/{meetingId}/rtms_app/status - update participant RTMS app status.</summary>
    public Task UpdateMeetingRtmsStatusAsync(string meetingId, MeetingRtmsStatusUpdateRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/live_meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/rtms_app/status", request, cancellationToken: cancellationToken);
}
