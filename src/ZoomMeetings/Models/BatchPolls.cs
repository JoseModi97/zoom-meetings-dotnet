using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Request body for POST /meetings/{meetingId}/batch_polls.</summary>
public class CreateBatchPollsRequest
{
    [JsonPropertyName("polls")]
    public List<Poll> Polls { get; set; } = new();
}

/// <summary>Response body for POST /meetings/{meetingId}/batch_polls.</summary>
public class CreateBatchPollsResult
{
    [JsonPropertyName("polls")]
    public List<Poll>? Polls { get; set; }
}
