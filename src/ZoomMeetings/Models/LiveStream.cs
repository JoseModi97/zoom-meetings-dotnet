using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body shared by GET /meetings/{meetingId}/livestream and GET /webinars/{webinarId}/livestream.</summary>
public class LiveStreamDetails
{
    [JsonPropertyName("page_url")]
    public string? PageUrl { get; set; }

    [JsonPropertyName("stream_key")]
    public string? StreamKey { get; set; }

    [JsonPropertyName("stream_url")]
    public string? StreamUrl { get; set; }

    [JsonPropertyName("resolution")]
    public string? Resolution { get; set; }
}

/// <summary>Request body shared by PATCH .../livestream for meetings and webinars.</summary>
public class UpdateLiveStreamRequest
{
    [JsonPropertyName("page_url")]
    public string PageUrl { get; set; } = string.Empty;

    [JsonPropertyName("stream_key")]
    public string StreamKey { get; set; } = string.Empty;

    [JsonPropertyName("stream_url")]
    public string StreamUrl { get; set; } = string.Empty;

    [JsonPropertyName("resolution")]
    public string? Resolution { get; set; }
}

/// <summary>Request body shared by PATCH .../livestream/status for meetings and webinars ("start" or "stop").</summary>
public class UpdateLiveStreamStatusRequest
{
    [JsonPropertyName("action")]
    public string Action { get; set; } = string.Empty;

    [JsonPropertyName("settings")]
    public LiveStreamStatusSettings? Settings { get; set; }
}

public class LiveStreamStatusSettings
{
    [JsonPropertyName("active_speaker_name")]
    public bool? ActiveSpeakerName { get; set; }

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    /// <summary>Meetings only: "follow_host" | "gallery_view" | "speaker_view". Ignored by webinars.</summary>
    [JsonPropertyName("layout")]
    public string? Layout { get; set; }

    /// <summary>"burnt-in" | "embedded" | "off".</summary>
    [JsonPropertyName("close_caption")]
    public string? CloseCaption { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}
