using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

/// <summary>Response/request body shared by GET/PATCH .../survey for meetings and webinars.</summary>
public class MeetingSurvey
{
    [JsonPropertyName("custom_survey")]
    public CustomSurvey? CustomSurvey { get; set; }

    [JsonPropertyName("show_in_the_browser")]
    public bool? ShowInTheBrowser { get; set; }

    /// <summary>Webinars only.</summary>
    [JsonPropertyName("show_in_the_follow_up_email")]
    public bool? ShowInTheFollowUpEmail { get; set; }

    [JsonPropertyName("third_party_survey")]
    public string? ThirdPartySurvey { get; set; }
}

public class CustomSurvey
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("anonymous")]
    public bool? Anonymous { get; set; }

    [JsonPropertyName("numbered_questions")]
    public bool? NumberedQuestions { get; set; }

    [JsonPropertyName("show_question_type")]
    public bool? ShowQuestionType { get; set; }

    [JsonPropertyName("feedback")]
    public string? Feedback { get; set; }

    [JsonPropertyName("questions")]
    public List<SurveyQuestion>? Questions { get; set; }
}

public class SurveyQuestion
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("answer_required")]
    public bool? AnswerRequired { get; set; }

    [JsonPropertyName("answers")]
    public List<string>? Answers { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}
