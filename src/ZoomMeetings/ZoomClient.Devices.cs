using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    // ==================== Zoom Rooms / ZDM Devices ====================

    /// <summary>GET /devices - list devices.</summary>
    public Task<ListDevicesResult?> ListDevicesAsync(
        string? searchText = null,
        string? platformOs = null,
        bool? isEnrolledInZdm = null,
        int? deviceType = null,
        string? deviceVendor = null,
        string? deviceModel = null,
        int? deviceStatus = null,
        int? pageSize = null,
        string? nextPageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (searchText != null) query["search_text"] = searchText;
        if (platformOs != null) query["platform_os"] = platformOs;
        if (isEnrolledInZdm != null) query["is_enrolled_in_zdm"] = isEnrolledInZdm.Value ? "true" : "false";
        if (deviceType != null) query["device_type"] = deviceType.Value.ToString();
        if (deviceVendor != null) query["device_vendor"] = deviceVendor;
        if (deviceModel != null) query["device_model"] = deviceModel;
        if (deviceStatus != null) query["device_status"] = deviceStatus.Value.ToString();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;

        return CallAsync<ListDevicesResult>(HttpMethod.Get, "/devices", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>POST /devices - add a new device.</summary>
    public Task CreateDeviceAsync(AddDeviceRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Post, "/devices", request, cancellationToken: cancellationToken);

    /// <summary>GET /devices/{deviceId} - get device detail.</summary>
    public Task<Device?> GetDeviceAsync(string deviceId, CancellationToken cancellationToken = default)
        => CallAsync<Device>(HttpMethod.Get, $"/devices/{Uri.EscapeDataString(deviceId)}", cancellationToken: cancellationToken);

    /// <summary>PATCH /devices/{deviceId} - update device.</summary>
    public Task UpdateDeviceAsync(string deviceId, UpdateDeviceRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/devices/{Uri.EscapeDataString(deviceId)}", request, cancellationToken: cancellationToken);

    /// <summary>DELETE /devices/{deviceId} - delete device.</summary>
    public Task DeleteDeviceAsync(string deviceId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/devices/{Uri.EscapeDataString(deviceId)}", cancellationToken: cancellationToken);

    /// <summary>PATCH /devices/{deviceId}/assign_group - assign a device to a group.</summary>
    public Task AssignDeviceGroupAsync(string deviceId, string groupId, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["group_id"] = groupId };
        return CallAsync(HttpMethods.Patch, $"/devices/{Uri.EscapeDataString(deviceId)}/assign_group", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>PATCH /devices/{deviceId}/assignment - change device association.</summary>
    public Task ChangeDeviceAssociationAsync(string deviceId, ChangeDeviceAssociationRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/devices/{Uri.EscapeDataString(deviceId)}/assignment", request, cancellationToken: cancellationToken);

    /// <summary>GET /devices/groups - get ZDM group info.</summary>
    public Task<ZdmGroupsResult?> ListDeviceGroupsAsync(int? pageSize = null, string? nextPageToken = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;

        return CallAsync<ZdmGroupsResult>(HttpMethod.Get, "/devices/groups", query: query, cancellationToken: cancellationToken);
    }

    // ==================== Zoom Phone Appliance (ZPA) ====================

    /// <summary>POST /devices/zpa/assignment - assign a device to a user or common area.</summary>
    public Task AssignZpaDeviceAsync(ZpaAssignmentRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Post, "/devices/zpa/assignment", request, cancellationToken: cancellationToken);

    /// <summary>GET /devices/zpa/settings - get Zoom Phone Appliance settings by user ID.</summary>
    public Task<ZpaUserSettingsResult?> GetZpaUserSettingsAsync(string? userId = null, CancellationToken cancellationToken = default)
    {
        var query = userId == null ? null : new Dictionary<string, string?> { ["user_id"] = userId };
        return CallAsync<ZpaUserSettingsResult>(HttpMethod.Get, "/devices/zpa/settings", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>POST /devices/zpa/upgrade - upgrade ZPA firmware or app.</summary>
    public Task UpgradeZpaAsync(ZpaUpgradeRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Post, "/devices/zpa/upgrade", request, cancellationToken: cancellationToken);

    /// <summary>DELETE /devices/zpa/vendors/{vendor}/mac_addresses/{macAddress} - delete ZPA device by vendor and MAC address.</summary>
    public Task DeleteZpaDeviceAsync(string vendor, string macAddress, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/devices/zpa/vendors/{Uri.EscapeDataString(vendor)}/mac_addresses/{Uri.EscapeDataString(macAddress)}", cancellationToken: cancellationToken);

    /// <summary>GET /devices/zpa/zdm_groups/{zdmGroupId}/versions - get ZPA version info.</summary>
    public Task<ZpaVersionsResult?> GetZpaVersionsAsync(string zdmGroupId, CancellationToken cancellationToken = default)
        => CallAsync<ZpaVersionsResult>(HttpMethod.Get, $"/devices/zpa/zdm_groups/{Uri.EscapeDataString(zdmGroupId)}/versions", cancellationToken: cancellationToken);

    // ==================== H.323 / SIP Devices ====================

    /// <summary>GET /h323/devices - list H.323/SIP devices.</summary>
    public Task<ListH323DevicesResult?> ListH323DevicesAsync(
        int? pageSize = null,
        int? pageNumber = null,
        string? nextPageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (pageNumber != null) query["page_number"] = pageNumber.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;

        return CallAsync<ListH323DevicesResult>(HttpMethod.Get, "/h323/devices", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>POST /h323/devices - create a H.323/SIP device.</summary>
    public Task<H323Device?> CreateH323DeviceAsync(CreateH323DeviceRequest request, CancellationToken cancellationToken = default)
        => CallAsync<H323Device>(HttpMethod.Post, "/h323/devices", request, cancellationToken: cancellationToken);

    /// <summary>DELETE /h323/devices/{deviceId} - delete a H.323/SIP device.</summary>
    public Task DeleteH323DeviceAsync(string deviceId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/h323/devices/{Uri.EscapeDataString(deviceId)}", cancellationToken: cancellationToken);

    /// <summary>PATCH /h323/devices/{deviceId} - update a H.323/SIP device.</summary>
    public Task UpdateH323DeviceAsync(string deviceId, UpdateH323DeviceRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/h323/devices/{Uri.EscapeDataString(deviceId)}", request, cancellationToken: cancellationToken);
}
