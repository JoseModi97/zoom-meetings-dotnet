using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Request body for POST /webinars/{webinarId}/batch_registrants.</summary>
public class AddBatchRegistrantsRequest
{
    [JsonPropertyName("auto_approve")]
    public bool? AutoApprove { get; set; }

    [JsonPropertyName("registrants")]
    public List<BatchRegistrantRequest> Registrants { get; set; } = new();
}

public class BatchRegistrantRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }
}

/// <summary>Response body for POST /webinars/{webinarId}/batch_registrants.</summary>
public class AddBatchRegistrantsResult
{
    [JsonPropertyName("registrants")]
    public List<BatchRegistrantResult>? Registrants { get; set; }
}

public class BatchRegistrantResult
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("join_url")]
    public string? JoinUrl { get; set; }

    [JsonPropertyName("registrant_id")]
    public string? RegistrantId { get; set; }
}
