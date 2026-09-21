using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Request body for PATCH /live_meetings/{meetingId}/chat/messages/{messageId}.</summary>
public class UpdateLiveChatMessageRequest
{
    [JsonPropertyName("message_content")]
    public string MessageContent { get; set; } = string.Empty;
}

/// <summary>Request body for PATCH /live_meetings/{meetingId}/events.</summary>
public class InMeetingControlRequest
{
    [JsonPropertyName("method")]
    public string Method { get; set; } = string.Empty;

    [JsonPropertyName("params")]
    public InMeetingControlParams? Params { get; set; }
}

public class InMeetingControlParams
{
    [JsonPropertyName("participant_user_id")]
    public string? ParticipantUserId { get; set; }

    [JsonPropertyName("participant_uuid")]
    public string? ParticipantUuid { get; set; }

    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("invite_options")]
    public InMeetingInviteOptions? InviteOptions { get; set; }

    [JsonPropertyName("call_type")]
    public string? CallType { get; set; }

    [JsonPropertyName("device_ip")]
    public string? DeviceIp { get; set; }

    [JsonPropertyName("h323_headers")]
    public InMeetingH323Headers? H323Headers { get; set; }

    [JsonPropertyName("sip_headers")]
    public InMeetingSipHeaders? SipHeaders { get; set; }

    [JsonPropertyName("waiting_room_title")]
    public string? WaitingRoomTitle { get; set; }

    [JsonPropertyName("waiting_room_description")]
    public string? WaitingRoomDescription { get; set; }

    [JsonPropertyName("ai_companion_mode")]
    public string? AiCompanionMode { get; set; }

    [JsonPropertyName("retain_meeting_transcript")]
    public bool? RetainMeetingTranscript { get; set; }

    [JsonPropertyName("delete_meeting_assets")]
    public bool? DeleteMeetingAssets { get; set; }
}

public class InMeetingInviteOptions
{
    [JsonPropertyName("require_greeting")]
    public bool? RequireGreeting { get; set; }

    [JsonPropertyName("require_pressing_one")]
    public bool? RequirePressingOne { get; set; }
}

public class InMeetingH323Headers
{
    [JsonPropertyName("from_display_name")]
    public string? FromDisplayName { get; set; }

    [JsonPropertyName("to_display_name")]
    public string? ToDisplayName { get; set; }
}

public class InMeetingSipHeaders
{
    [JsonPropertyName("from_display_name")]
    public string? FromDisplayName { get; set; }

    [JsonPropertyName("to_display_name")]
    public string? ToDisplayName { get; set; }

    [JsonPropertyName("from_uri")]
    public string? FromUri { get; set; }

    [JsonPropertyName("additional_headers")]
    public List<InMeetingCustomHeader>? AdditionalHeaders { get; set; }
}

public class InMeetingCustomHeader
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}

/// <summary>Request body for PATCH /live_meetings/{meetingId}/rtms_app/status.</summary>
public class MeetingRtmsStatusUpdateRequest
{
    [JsonPropertyName("action")]
    public string Action { get; set; } = string.Empty;

    [JsonPropertyName("settings")]
    public MeetingRtmsSettings? Settings { get; set; }
}

public class MeetingRtmsSettings
{
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = string.Empty;

    [JsonPropertyName("participant_user_id")]
    public string? ParticipantUserId { get; set; }
}
