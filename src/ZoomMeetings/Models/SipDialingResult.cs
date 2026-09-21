using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body shared by POST .../sip_dialing for meetings and webinars.</summary>
public class SipDialingResult
{
    [JsonPropertyName("sip_dialing")]
    public string? SipDialing { get; set; }

    [JsonPropertyName("paid_crc_plan_participant")]
    public bool? PaidCrcPlanParticipant { get; set; }

    [JsonPropertyName("participant_identifier_code")]
    public string? ParticipantIdentifierCode { get; set; }

    [JsonPropertyName("expire_in")]
    public long? ExpireIn { get; set; }
}
