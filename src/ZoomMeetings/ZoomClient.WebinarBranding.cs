using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>GET /webinars/{webinarId}/branding - get a webinar's session branding (wallpaper, virtual backgrounds, name tags).</summary>
    public Task<WebinarBranding?> GetWebinarBrandingAsync(string webinarId, CancellationToken cancellationToken = default)
        => CallAsync<WebinarBranding>(HttpMethod.Get, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/branding", cancellationToken: cancellationToken);

    /// <summary>POST /webinars/{webinarId}/branding/name_tags - create a branding name tag.</summary>
    public Task<WebinarBrandingNameTag?> CreateWebinarBrandingNameTagAsync(string webinarId, NameTagRequest request, CancellationToken cancellationToken = default)
        => CallAsync<WebinarBrandingNameTag>(HttpMethod.Post, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/branding/name_tags", request, cancellationToken: cancellationToken);

    /// <summary>DELETE /webinars/{webinarId}/branding/name_tags - delete one or more branding name tags.</summary>
    public Task DeleteWebinarBrandingNameTagsAsync(string webinarId, IEnumerable<string> nameTagIds, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["name_tag_ids"] = string.Join(",", nameTagIds) };
        return CallAsync(HttpMethod.Delete, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/branding/name_tags", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>PATCH /webinars/{webinarId}/branding/name_tags/{nameTagId} - update a branding name tag.</summary>
    public Task UpdateWebinarBrandingNameTagAsync(string webinarId, string nameTagId, NameTagRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/branding/name_tags/{Uri.EscapeDataString(nameTagId)}", request, cancellationToken: cancellationToken);

    /// <summary>POST /webinars/{webinarId}/branding/virtual_backgrounds - upload a branding virtual background image.</summary>
    public Task<WebinarBrandingVirtualBackground?> UploadWebinarBrandingVirtualBackgroundAsync(
        string webinarId, Stream fileStream, string fileName, bool? isDefault = null, bool? setDefaultForAllPanelists = null, CancellationToken cancellationToken = default)
    {
        var fields = new Dictionary<string, string>();
        if (isDefault != null) fields["default"] = isDefault.Value.ToString().ToLowerInvariant();
        if (setDefaultForAllPanelists != null) fields["set_default_for_all_panelists"] = setDefaultForAllPanelists.Value.ToString().ToLowerInvariant();
        return UploadFileAsync<WebinarBrandingVirtualBackground>(
            HttpMethod.Post, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/branding/virtual_backgrounds", fileStream, fileName, additionalFields: fields, cancellationToken: cancellationToken);
    }

    /// <summary>PATCH /webinars/{webinarId}/branding/virtual_backgrounds - set the default branding virtual background.</summary>
    public Task SetDefaultWebinarBrandingVirtualBackgroundAsync(string webinarId, string virtualBackgroundId, bool? setDefaultForAllPanelists = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["id"] = virtualBackgroundId };
        if (setDefaultForAllPanelists != null) query["set_default_for_all_panelists"] = setDefaultForAllPanelists.Value.ToString().ToLowerInvariant();
        return CallAsync(HttpMethods.Patch, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/branding/virtual_backgrounds", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>DELETE /webinars/{webinarId}/branding/virtual_backgrounds - delete one or more branding virtual backgrounds.</summary>
    public Task DeleteWebinarBrandingVirtualBackgroundsAsync(string webinarId, IEnumerable<string> ids, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["ids"] = string.Join(",", ids) };
        return CallAsync(HttpMethod.Delete, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/branding/virtual_backgrounds", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>POST /webinars/{webinarId}/branding/wallpaper - upload a branding wallpaper image.</summary>
    public Task<WebinarBrandingWallpaper?> UploadWebinarBrandingWallpaperAsync(string webinarId, Stream fileStream, string fileName, CancellationToken cancellationToken = default)
        => UploadFileAsync<WebinarBrandingWallpaper>(HttpMethod.Post, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/branding/wallpaper", fileStream, fileName, cancellationToken: cancellationToken);

    /// <summary>DELETE /webinars/{webinarId}/branding/wallpaper - delete the branding wallpaper.</summary>
    public Task DeleteWebinarBrandingWallpaperAsync(string webinarId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/branding/wallpaper", cancellationToken: cancellationToken);
}
