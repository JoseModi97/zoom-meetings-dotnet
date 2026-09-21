using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>SIP phone object.</summary>
public class SipPhone
{
    [JsonPropertyName("phone_id")]
    public string? PhoneId { get; set; }

    [JsonPropertyName("authorization_name")]
    public string? AuthorizationName { get; set; }

    [JsonPropertyName("domain")]
    public string? Domain { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("registration_expire_time")]
    public int? RegistrationExpireTime { get; set; }

    [JsonPropertyName("user_email")]
    public string? UserEmail { get; set; }

    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }

    [JsonPropertyName("voice_mail")]
    public string? VoiceMail { get; set; }

    [JsonPropertyName("display_number")]
    public string? DisplayNumber { get; set; }

    [JsonPropertyName("server")]
    public string? Server { get; set; }

    [JsonPropertyName("server_2")]
    public string? Server2 { get; set; }

    [JsonPropertyName("server_3")]
    public string? Server3 { get; set; }
}

/// <summary>Response body for GET /sip_phones/phones.</summary>
public class ListSipPhonesResult
{
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("next_page_token")]
    public string? NextPageToken { get; set; }

    [JsonPropertyName("phones")]
    public List<SipPhone>? Phones { get; set; }
}

/// <summary>Request body for POST /sip_phones/phones.</summary>
public class EnableSipPhoneRequest
{
    [JsonPropertyName("authorization_name")]
    public string AuthorizationName { get; set; } = string.Empty;

    [JsonPropertyName("domain")]
    public string Domain { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("registration_expire_time")]
    public int? RegistrationExpireTime { get; set; }

    [JsonPropertyName("user_email")]
    public string UserEmail { get; set; } = string.Empty;

    [JsonPropertyName("user_name")]
    public string UserName { get; set; } = string.Empty;

    [JsonPropertyName("voice_mail")]
    public string? VoiceMail { get; set; }

    [JsonPropertyName("display_number")]
    public string? DisplayNumber { get; set; }

    [JsonPropertyName("server")]
    public string? Server { get; set; }

    [JsonPropertyName("server_2")]
    public string? Server2 { get; set; }

    [JsonPropertyName("server_3")]
    public string? Server3 { get; set; }
}

/// <summary>Request body for PATCH /sip_phones/phones/{phoneId}.</summary>
public class UpdateSipPhoneRequest
{
    [JsonPropertyName("authorization_name")]
    public string? AuthorizationName { get; set; }

    [JsonPropertyName("domain")]
    public string? Domain { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("registration_expire_time")]
    public int? RegistrationExpireTime { get; set; }

    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }

    [JsonPropertyName("voice_mail")]
    public string? VoiceMail { get; set; }

    [JsonPropertyName("display_number")]
    public string? DisplayNumber { get; set; }

    [JsonPropertyName("server")]
    public string? Server { get; set; }

    [JsonPropertyName("server_2")]
    public string? Server2 { get; set; }

    [JsonPropertyName("server_3")]
    public string? Server3 { get; set; }
}
