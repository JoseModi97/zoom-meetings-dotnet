using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>A meeting or webinar poll.</summary>
public class Poll
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("anonymous")]
    public bool? Anonymous { get; set; }

    [JsonPropertyName("poll_type")]
    public int? PollType { get; set; }

    [JsonPropertyName("questions")]
    public List<PollQuestion>? Questions { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}

public class PollQuestion
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("answer_required")]
    public bool? AnswerRequired { get; set; }

    [JsonPropertyName("answers")]
    public List<string>? Answers { get; set; }
}

/// <summary>Response body for listing meeting or webinar polls.</summary>
public class ListPollsResult
{
    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }

    [JsonPropertyName("polls")]
    public List<Poll>? Polls { get; set; }
}
