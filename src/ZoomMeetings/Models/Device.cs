using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Zoom device object.</summary>
public class Device
{
    [JsonPropertyName("device_id")]
    public string? DeviceId { get; set; }

    [JsonPropertyName("device_name")]
    public string? DeviceName { get; set; }

    [JsonPropertyName("mac_address")]
    public string? MacAddress { get; set; }

    [JsonPropertyName("serial_number")]
    public string? SerialNumber { get; set; }

    [JsonPropertyName("vendor")]
    public string? Vendor { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("platform_os")]
    public string? PlatformOs { get; set; }

    [JsonPropertyName("app_version")]
    public string? AppVersion { get; set; }

    [JsonPropertyName("tag")]
    public string? Tag { get; set; }

    [JsonPropertyName("enrolled_in_zdm")]
    public bool? EnrolledInZdm { get; set; }

    [JsonPropertyName("connected_to_zdm")]
    public bool? ConnectedToZdm { get; set; }

    [JsonPropertyName("room_id")]
    public string? RoomId { get; set; }

    [JsonPropertyName("room_name")]
    public string? RoomName { get; set; }

    [JsonPropertyName("device_type")]
    public int? DeviceType { get; set; }

    [JsonPropertyName("sdk_version")]
    public string? SdkVersion { get; set; }

    [JsonPropertyName("device_status")]
    public int? DeviceStatus { get; set; }

    [JsonPropertyName("last_online")]
    public string? LastOnline { get; set; }

    [JsonPropertyName("user_email")]
    public string? UserEmail { get; set; }
}

/// <summary>Response body for GET /devices.</summary>
public class ListDevicesResult
{
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("devices")]
    public List<Device>? Devices { get; set; }
}

/// <summary>Request body for POST /devices.</summary>
public class AddDeviceRequest
{
    [JsonPropertyName("device_name")]
    public string DeviceName { get; set; } = string.Empty;

    [JsonPropertyName("mac_address")]
    public string MacAddress { get; set; } = string.Empty;

    [JsonPropertyName("serial_number")]
    public string SerialNumber { get; set; } = string.Empty;

    [JsonPropertyName("vendor")]
    public string Vendor { get; set; } = string.Empty;

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("room_id")]
    public string? RoomId { get; set; }

    [JsonPropertyName("user_email")]
    public string? UserEmail { get; set; }

    [JsonPropertyName("device_type")]
    public int? DeviceType { get; set; }

    [JsonPropertyName("tag")]
    public string? Tag { get; set; }

    [JsonPropertyName("zdm_group_id")]
    public string? ZdmGroupId { get; set; }

    [JsonPropertyName("extension_number")]
    public string? ExtensionNumber { get; set; }
}

/// <summary>Request body for PATCH /devices/{deviceId}.</summary>
public class UpdateDeviceRequest
{
    [JsonPropertyName("device_name")]
    public string? DeviceName { get; set; }

    [JsonPropertyName("tag")]
    public string? Tag { get; set; }

    [JsonPropertyName("room_id")]
    public string? RoomId { get; set; }

    [JsonPropertyName("device_type")]
    public int? DeviceType { get; set; }
}

/// <summary>Request body for PATCH /devices/{deviceId}/assignment.</summary>
public class ChangeDeviceAssociationRequest
{
    [JsonPropertyName("room_id")]
    public string? RoomId { get; set; }

    [JsonPropertyName("app_type")]
    public string? AppType { get; set; }
}

/// <summary>Response body for GET /devices/groups.</summary>
public class ZdmGroupsResult
{
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("groups")]
    public List<ZdmGroupItem>? Groups { get; set; }
}

public class ZdmGroupItem
{
    [JsonPropertyName("zdm_group_id")]
    public string? ZdmGroupId { get; set; }

    [JsonPropertyName("zdm_group_name")]
    public string? ZdmGroupName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

/// <summary>Request body for POST /devices/zpa/assignment.</summary>
public class ZpaAssignmentRequest
{
    [JsonPropertyName("extension_number")]
    public string? ExtensionNumber { get; set; }

    [JsonPropertyName("mac_address")]
    public string MacAddress { get; set; } = string.Empty;

    [JsonPropertyName("vendor")]
    public string Vendor { get; set; } = string.Empty;
}

/// <summary>Response body for GET /devices/zpa/settings.</summary>
public class ZpaUserSettingsResult
{
    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }

    [JsonPropertyName("device_infos")]
    public List<ZpaDeviceInfo>? DeviceInfos { get; set; }
}

public class ZpaDeviceInfo
{
    [JsonPropertyName("device_id")]
    public string? DeviceId { get; set; }

    [JsonPropertyName("mac_address")]
    public string? MacAddress { get; set; }

    [JsonPropertyName("vendor")]
    public string? Vendor { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }
}

/// <summary>Request body for POST /devices/zpa/upgrade.</summary>
public class ZpaUpgradeRequest
{
    [JsonPropertyName("zdm_group_id")]
    public string? ZdmGroupId { get; set; }

    [JsonPropertyName("data")]
    public List<ZpaUpgradeData>? Data { get; set; }
}

public class ZpaUpgradeData
{
    [JsonPropertyName("vendor")]
    public string? Vendor { get; set; }

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("version_type")]
    public string? VersionType { get; set; }

    [JsonPropertyName("target_version")]
    public string? TargetVersion { get; set; }
}

/// <summary>Response body for GET /devices/zpa/zdm_groups/{zdmGroupId}/versions.</summary>
public class ZpaVersionsResult
{
    [JsonPropertyName("firmware_versions")]
    public List<string>? FirmwareVersions { get; set; }

    [JsonPropertyName("app_versions")]
    public List<string>? AppVersions { get; set; }
}
