using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /webinars/{webinarId}/branding.</summary>
public class WebinarBranding
{
    [JsonPropertyName("wallpaper")]
    public WebinarBrandingWallpaperRef? Wallpaper { get; set; }

    [JsonPropertyName("virtual_backgrounds")]
    public List<WebinarBrandingVirtualBackground>? VirtualBackgrounds { get; set; }

    [JsonPropertyName("name_tags")]
    public List<WebinarBrandingNameTag>? NameTags { get; set; }
}

public class WebinarBrandingWallpaperRef
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class WebinarBrandingNameTag
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("text_color")]
    public string? TextColor { get; set; }

    [JsonPropertyName("accent_color")]
    public string? AccentColor { get; set; }

    [JsonPropertyName("background_color")]
    public string? BackgroundColor { get; set; }

    [JsonPropertyName("is_default")]
    public bool? IsDefault { get; set; }
}

/// <summary>Request body for POST/PATCH .../branding/name_tags.</summary>
public class NameTagRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("text_color")]
    public string? TextColor { get; set; }

    [JsonPropertyName("accent_color")]
    public string? AccentColor { get; set; }

    [JsonPropertyName("background_color")]
    public string? BackgroundColor { get; set; }

    [JsonPropertyName("is_default")]
    public bool? IsDefault { get; set; }

    [JsonPropertyName("set_default_for_all_panelists")]
    public bool? SetDefaultForAllPanelists { get; set; }
}

public class WebinarBrandingVirtualBackground
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("is_default")]
    public bool? IsDefault { get; set; }

    [JsonPropertyName("size")]
    public long? Size { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public class WebinarBrandingWallpaper
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("size")]
    public long? Size { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
