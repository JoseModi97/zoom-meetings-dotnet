using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Account-level TSP settings.</summary>
public class AccountTspSettings
{
    [JsonPropertyName("tsp_enabled")]
    public bool? TspEnabled { get; set; }

    [JsonPropertyName("tsp_provider")]
    public string? TspProvider { get; set; }

    [JsonPropertyName("tsp_bridge")]
    public string? TspBridge { get; set; }

    [JsonPropertyName("modify_credential_forbidden")]
    public bool? ModifyCredentialForbidden { get; set; }

    [JsonPropertyName("dial_in_number_unrestricted")]
    public bool? DialInNumberUnrestricted { get; set; }

    [JsonPropertyName("enable")]
    public bool? Enable { get; set; }

    [JsonPropertyName("master_account_setting_extended")]
    public bool? MasterAccountSettingExtended { get; set; }

    [JsonPropertyName("dial_in_numbers")]
    public List<TspDialInNumber>? DialInNumbers { get; set; }
}

/// <summary>Request body for PATCH /tsp.</summary>
public class UpdateAccountTspSettingsRequest
{
    [JsonPropertyName("tsp_enabled")]
    public bool? TspEnabled { get; set; }

    [JsonPropertyName("tsp_provider")]
    public string? TspProvider { get; set; }

    [JsonPropertyName("tsp_bridge")]
    public string? TspBridge { get; set; }

    [JsonPropertyName("modify_credential_forbidden")]
    public bool? ModifyCredentialForbidden { get; set; }

    [JsonPropertyName("dial_in_number_unrestricted")]
    public bool? DialInNumberUnrestricted { get; set; }

    [JsonPropertyName("enable")]
    public bool? Enable { get; set; }

    [JsonPropertyName("master_account_setting_extended")]
    public bool? MasterAccountSettingExtended { get; set; }
}

public class TspDialInNumber
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("country_label")]
    public string? CountryLabel { get; set; }
}

/// <summary>User's TSP account.</summary>
public class UserTspAccount
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("conference_code")]
    public string ConferenceCode { get; set; } = string.Empty;

    [JsonPropertyName("leader_pin")]
    public string LeaderPin { get; set; } = string.Empty;

    [JsonPropertyName("tsp_bridge")]
    public string? TspBridge { get; set; }

    [JsonPropertyName("dial_in_numbers")]
    public List<TspDialInNumber>? DialInNumbers { get; set; }
}

/// <summary>Response body for GET /users/{userId}/tsp.</summary>
public class ListUserTspsResult
{
    [JsonPropertyName("tsp_accounts")]
    public List<UserTspAccount>? TspAccounts { get; set; }
}

/// <summary>Request body for POST /users/{userId}/tsp.</summary>
public class CreateUserTspRequest
{
    [JsonPropertyName("conference_code")]
    public string ConferenceCode { get; set; } = string.Empty;

    [JsonPropertyName("leader_pin")]
    public string LeaderPin { get; set; } = string.Empty;

    [JsonPropertyName("tsp_bridge")]
    public string? TspBridge { get; set; }

    [JsonPropertyName("dial_in_numbers")]
    public List<TspDialInNumber>? DialInNumbers { get; set; }
}

/// <summary>Request body for PATCH /users/{userId}/tsp/{tspId}.</summary>
public class UpdateUserTspRequest
{
    [JsonPropertyName("conference_code")]
    public string ConferenceCode { get; set; } = string.Empty;

    [JsonPropertyName("leader_pin")]
    public string LeaderPin { get; set; } = string.Empty;

    [JsonPropertyName("tsp_bridge")]
    public string? TspBridge { get; set; }

    [JsonPropertyName("dial_in_numbers")]
    public List<TspDialInNumber>? DialInNumbers { get; set; }
}

/// <summary>Request body for PATCH /users/{userId}/tsp/settings.</summary>
public class UpdateUserTspUrlRequest
{
    [JsonPropertyName("audio_url")]
    public string AudioUrl { get; set; } = string.Empty;
}
