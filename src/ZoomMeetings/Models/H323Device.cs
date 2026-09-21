using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>H.323/SIP device object.</summary>
public class H323Device
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("protocol")]
    public string Protocol { get; set; } = string.Empty;

    [JsonPropertyName("ip")]
    public string Ip { get; set; } = string.Empty;

    [JsonPropertyName("encryption")]
    public string Encryption { get; set; } = string.Empty;
}

/// <summary>Response body for GET /h323/devices.</summary>
public class ListH323DevicesResult
{
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("page_count")]
    public int? PageCount { get; set; }

    [JsonPropertyName("page_number")]
    public int? PageNumber { get; set; }

    [JsonPropertyName("total_records")]
    public int? TotalRecords { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("devices")]
    public List<H323Device>? Devices { get; set; }
}

/// <summary>Request body for POST /h323/devices.</summary>
public class CreateH323DeviceRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("protocol")]
    public string Protocol { get; set; } = string.Empty;

    [JsonPropertyName("ip")]
    public string Ip { get; set; } = string.Empty;

    [JsonPropertyName("encryption")]
    public string Encryption { get; set; } = string.Empty;
}

/// <summary>Request body for PATCH /h323/devices/{deviceId}.</summary>
public class UpdateH323DeviceRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("protocol")]
    public string? Protocol { get; set; }

    [JsonPropertyName("ip")]
    public string? Ip { get; set; }

    [JsonPropertyName("encryption")]
    public string? Encryption { get; set; }
}
