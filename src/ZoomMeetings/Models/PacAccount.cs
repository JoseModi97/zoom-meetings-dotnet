using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response body for GET /users/{userId}/pac.</summary>
public class ListPacAccountsResult
{
    [JsonPropertyName("pac_accounts")]
    public List<PacAccount>? PacAccounts { get; set; }
}

public class PacAccount
{
    [JsonPropertyName("conference_id")]
    public string? ConferenceId { get; set; }

    [JsonPropertyName("dedicated_dial_in_number")]
    public string? DedicatedDialInNumber { get; set; }

    [JsonPropertyName("global_dial_in_numbers")]
    public string? GlobalDialInNumbers { get; set; }

    [JsonPropertyName("listen_only_password")]
    public string? ListenOnlyPassword { get; set; }

    [JsonPropertyName("participant_password")]
    public string? ParticipantPassword { get; set; }
}
