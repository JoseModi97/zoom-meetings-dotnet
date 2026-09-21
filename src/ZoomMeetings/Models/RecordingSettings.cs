using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Get/update meeting recording settings (GET/PATCH /meetings/{meetingId}/recordings/settings).</summary>
public class RecordingSettings
{
    [JsonPropertyName("approval_type")]
    public int? ApprovalType { get; set; }

    [JsonPropertyName("on_demand")]
    public bool? OnDemand { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("recording_authentication")]
    public bool? RecordingAuthentication { get; set; }

    [JsonPropertyName("send_email_to_host")]
    public bool? SendEmailToHost { get; set; }

    [JsonPropertyName("share_recording")]
    public string? ShareRecording { get; set; }

    [JsonPropertyName("show_social_share_buttons")]
    public bool? ShowSocialShareButtons { get; set; }

    [JsonPropertyName("topic")]
    public string? Topic { get; set; }

    [JsonPropertyName("viewer_download")]
    public bool? ViewerDownload { get; set; }

    [JsonPropertyName("auto_delete")]
    public bool? AutoDelete { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}
