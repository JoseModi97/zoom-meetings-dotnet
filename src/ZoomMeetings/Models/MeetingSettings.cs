using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>
/// A trimmed, commonly-used subset of Zoom's meeting "settings" object, which has 66-69 properties in
/// the full spec. The rest round-trip through <see cref="ExtraData"/> so nothing is lost when you
/// read a meeting, tweak one known field, and send it back.
/// </summary>
public class MeetingSettings
{
    [JsonPropertyName("host_video")]
    public bool? HostVideo { get; set; }

    [JsonPropertyName("participant_video")]
    public bool? ParticipantVideo { get; set; }

    [JsonPropertyName("join_before_host")]
    public bool? JoinBeforeHost { get; set; }

    [JsonPropertyName("mute_upon_entry")]
    public bool? MuteUponEntry { get; set; }

    [JsonPropertyName("waiting_room")]
    public bool? WaitingRoom { get; set; }

    [JsonPropertyName("watermark")]
    public bool? Watermark { get; set; }

    [JsonPropertyName("audio")]
    public string? Audio { get; set; }

    [JsonPropertyName("auto_recording")]
    public string? AutoRecording { get; set; }

    [JsonPropertyName("approval_type")]
    public int? ApprovalType { get; set; }

    [JsonPropertyName("registration_type")]
    public int? RegistrationType { get; set; }

    [JsonPropertyName("meeting_authentication")]
    public bool? MeetingAuthentication { get; set; }

    [JsonPropertyName("close_registration")]
    public bool? CloseRegistration { get; set; }

    [JsonPropertyName("allow_multiple_devices")]
    public bool? AllowMultipleDevices { get; set; }

    [JsonPropertyName("alternative_hosts")]
    public string? AlternativeHosts { get; set; }

    [JsonPropertyName("use_pmi")]
    public bool? UsePmi { get; set; }

    [JsonPropertyName("email_notification")]
    public bool? EmailNotification { get; set; }

    [JsonPropertyName("contact_email")]
    public string? ContactEmail { get; set; }

    [JsonPropertyName("contact_name")]
    public string? ContactName { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}
