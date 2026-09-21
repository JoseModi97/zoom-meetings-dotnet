using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Request body shared by POST .../invite_links for meetings and webinars.</summary>
public class CreateInviteLinksRequest
{
    [JsonPropertyName("attendees")]
    public List<InviteLinkAttendeeRequest>? Attendees { get; set; }

    [JsonPropertyName("ttl")]
    public long? Ttl { get; set; }
}

public class InviteLinkAttendeeRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("disable_video")]
    public bool? DisableVideo { get; set; }

    [JsonPropertyName("disable_audio")]
    public bool? DisableAudio { get; set; }
}

/// <summary>Response body shared by POST .../invite_links for meetings and webinars.</summary>
public class CreateInviteLinksResult
{
    [JsonPropertyName("attendees")]
    public List<InviteLinkAttendeeResult>? Attendees { get; set; }
}

public class InviteLinkAttendeeResult
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("join_url")]
    public string? JoinUrl { get; set; }
}
