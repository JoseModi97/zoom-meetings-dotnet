using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZoomMeetings.Models;

// Models for the /report/* endpoints. These are read-only, page-once-and-done reports, so each
// result type keeps the commonly-used fields typed with an [JsonExtensionData] catch-all - Zoom's
// report payloads are wide (10-25 columns) and grow over time as Zoom adds reporting fields.

/// <summary>GET /report/activities.</summary>
public class SignInSignOutActivityReportResult
{
    [JsonPropertyName("from")] public string? From { get; set; }
    [JsonPropertyName("to")] public string? To { get; set; }
    [JsonPropertyName("page_size")] public int PageSize { get; set; }
    [JsonPropertyName("next_page_token")] public string? NextPageToken { get; set; }
    [JsonPropertyName("activity_logs")] public List<ActivityLogEntry>? ActivityLogs { get; set; }
}

public class ActivityLogEntry
{
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("client_type")] public string? ClientType { get; set; }
    [JsonPropertyName("ip_address")] public string? IpAddress { get; set; }
    [JsonPropertyName("time")] public DateTimeOffset? Time { get; set; }
    [JsonPropertyName("version")] public string? Version { get; set; }
}

/// <summary>GET /report/billing.</summary>
public class BillingReportResult
{
    [JsonPropertyName("currency")] public string? Currency { get; set; }
    [JsonPropertyName("billing_reports")] public List<BillingReportEntry>? BillingReports { get; set; }
}

public class BillingReportEntry
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("start_date")] public DateTimeOffset? StartDate { get; set; }
    [JsonPropertyName("end_date")] public DateTimeOffset? EndDate { get; set; }
    [JsonPropertyName("total_amount")] public decimal? TotalAmount { get; set; }
    [JsonPropertyName("tax_amount")] public decimal? TaxAmount { get; set; }
}

/// <summary>GET /report/billing/invoices.</summary>
public class BillingInvoicesReportResult
{
    [JsonPropertyName("currency")] public string? Currency { get; set; }
    [JsonPropertyName("invoices")] public List<BillingInvoiceEntry>? Invoices { get; set; }
}

public class BillingInvoiceEntry
{
    [JsonPropertyName("invoice_number")] public string? InvoiceNumber { get; set; }
    [JsonPropertyName("invoice_charge_name")] public string? InvoiceChargeName { get; set; }
    [JsonPropertyName("start_date")] public DateTimeOffset? StartDate { get; set; }
    [JsonPropertyName("end_date")] public DateTimeOffset? EndDate { get; set; }
    [JsonPropertyName("quantity")] public int? Quantity { get; set; }
    [JsonPropertyName("total_amount")] public decimal? TotalAmount { get; set; }
    [JsonPropertyName("tax_amount")] public decimal? TaxAmount { get; set; }
}

/// <summary>GET /report/cloud_recording.</summary>
public class CloudRecordingUsageReportResult
{
    [JsonPropertyName("from")] public string? From { get; set; }
    [JsonPropertyName("to")] public string? To { get; set; }
    [JsonPropertyName("cloud_recording_storage")] public List<CloudRecordingStorageEntry>? CloudRecordingStorage { get; set; }
}

public class CloudRecordingStorageEntry
{
    [JsonPropertyName("date")] public string? Date { get; set; }
    [JsonPropertyName("usage")] public string? Usage { get; set; }
    [JsonPropertyName("plan_usage")] public string? PlanUsage { get; set; }
    [JsonPropertyName("free_usage")] public string? FreeUsage { get; set; }
}

/// <summary>GET /report/daily.</summary>
public class DailyUsageReportResult
{
    [JsonPropertyName("year")] public int Year { get; set; }
    [JsonPropertyName("month")] public int Month { get; set; }
    [JsonPropertyName("dates")] public List<DailyUsageEntry>? Dates { get; set; }
}

public class DailyUsageEntry
{
    [JsonPropertyName("date")] public string? Date { get; set; }
    [JsonPropertyName("new_users")] public int? NewUsers { get; set; }
    [JsonPropertyName("meetings")] public int? Meetings { get; set; }
    [JsonPropertyName("participants")] public int? Participants { get; set; }
    [JsonPropertyName("meeting_minutes")] public int? MeetingMinutes { get; set; }
}

/// <summary>GET /report/disclaimer.</summary>
public class DisclaimerReportResult
{
    [JsonPropertyName("page_size")] public int PageSize { get; set; }
    [JsonPropertyName("next_page_token")] public string? NextPageToken { get; set; }
    [JsonPropertyName("disclaimer_records")] public List<DisclaimerRecord>? DisclaimerRecords { get; set; }
}

public class DisclaimerRecord
{
    [JsonPropertyName("disclaimer_status")] public string? DisclaimerStatus { get; set; }
    [JsonPropertyName("disclaimer_type")] public string? DisclaimerType { get; set; }
    [JsonPropertyName("time")] public DateTimeOffset? Time { get; set; }
    [JsonPropertyName("user_email")] public string? UserEmail { get; set; }
    [JsonPropertyName("meeting_number")] public string? MeetingNumber { get; set; }
    [JsonPropertyName("meeting_id")] public string? MeetingId { get; set; }
    [JsonPropertyName("client_type")] public string? ClientType { get; set; }
    [JsonPropertyName("display_name")] public string? DisplayName { get; set; }
    [JsonExtensionData] public Dictionary<string, JsonElement>? ExtraData { get; set; }
}

/// <summary>GET /report/history_meetings.</summary>
public class HistoryMeetingsReportResult
{
    [JsonPropertyName("page_size")] public int PageSize { get; set; }
    [JsonPropertyName("next_page_token")] public string? NextPageToken { get; set; }
    [JsonPropertyName("history_meetings")] public List<HistoryMeetingEntry>? HistoryMeetings { get; set; }
}

public class HistoryMeetingEntry
{
    [JsonPropertyName("meeting_uuid")] public string? MeetingUuid { get; set; }
    [JsonPropertyName("meeting_id")] public string? MeetingId { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("topic")] public string? Topic { get; set; }
    [JsonPropertyName("host_display_name")] public string? HostDisplayName { get; set; }
    [JsonPropertyName("host_email")] public string? HostEmail { get; set; }
    [JsonPropertyName("start_time")] public DateTimeOffset? StartTime { get; set; }
    [JsonPropertyName("end_time")] public DateTimeOffset? EndTime { get; set; }
    [JsonPropertyName("duration")] public int? Duration { get; set; }
    [JsonPropertyName("participants")] public int? Participants { get; set; }
    [JsonPropertyName("total_participant_minutes")] public int? TotalParticipantMinutes { get; set; }
    [JsonExtensionData] public Dictionary<string, JsonElement>? ExtraData { get; set; }
}

/// <summary>GET /report/meeting_activities.</summary>
public class MeetingActivitiesReportResult
{
    [JsonPropertyName("page_size")] public double PageSize { get; set; }
    [JsonPropertyName("next_page_token")] public string? NextPageToken { get; set; }
    [JsonPropertyName("meeting_activity_logs")] public List<MeetingActivityLogEntry>? MeetingActivityLogs { get; set; }
}

public class MeetingActivityLogEntry
{
    [JsonPropertyName("meeting_number")] public string? MeetingNumber { get; set; }
    [JsonPropertyName("activity_time")] public DateTimeOffset? ActivityTime { get; set; }
    [JsonPropertyName("operator")] public string? Operator { get; set; }
    [JsonPropertyName("operator_email")] public string? OperatorEmail { get; set; }
    [JsonPropertyName("activity_category")] public string? ActivityCategory { get; set; }
    [JsonPropertyName("activity_detail")] public string? ActivityDetail { get; set; }
}

/// <summary>GET /report/meetings/{meetingId}/polls and GET /report/webinars/{webinarId}/polls.</summary>
public class PollReportResult
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("uuid")] public string? Uuid { get; set; }
    [JsonPropertyName("start_time")] public DateTimeOffset? StartTime { get; set; }
    [JsonPropertyName("questions")] public List<PollReportQuestion>? Questions { get; set; }
}

public class PollReportQuestion
{
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("first_name")] public string? FirstName { get; set; }
    [JsonPropertyName("last_name")] public string? LastName { get; set; }
    [JsonPropertyName("question_details")] public List<QaQuestionDetail>? QuestionDetails { get; set; }
}

/// <summary>GET /report/meetings/{meetingId}/survey and GET /report/webinars/{webinarId}/survey.</summary>
public class SurveyReportResult
{
    [JsonPropertyName("meeting_id")] public long? MeetingId { get; set; }
    [JsonPropertyName("meeting_uuid")] public string? MeetingUuid { get; set; }
    [JsonPropertyName("webinar_id")] public long? WebinarId { get; set; }
    [JsonPropertyName("webinar_uuid")] public string? WebinarUuid { get; set; }
    [JsonPropertyName("start_time")] public DateTimeOffset? StartTime { get; set; }
    [JsonPropertyName("survey_id")] public string? SurveyId { get; set; }
    [JsonPropertyName("survey_name")] public string? SurveyName { get; set; }
    [JsonPropertyName("survey_answers")] public List<SurveyAnswerEntry>? SurveyAnswers { get; set; }
}

public class SurveyAnswerEntry
{
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("first_name")] public string? FirstName { get; set; }
    [JsonPropertyName("last_name")] public string? LastName { get; set; }
    [JsonPropertyName("answer_details")] public List<QaQuestionDetail>? AnswerDetails { get; set; }
}

/// <summary>GET /report/operationlogs.</summary>
public class OperationLogsReportResult
{
    [JsonPropertyName("page_size")] public int PageSize { get; set; }
    [JsonPropertyName("next_page_token")] public string? NextPageToken { get; set; }
    [JsonPropertyName("operation_logs")] public List<OperationLogEntry>? OperationLogs { get; set; }
}

public class OperationLogEntry
{
    [JsonPropertyName("time")] public DateTimeOffset? Time { get; set; }
    [JsonPropertyName("operator")] public string? Operator { get; set; }
    [JsonPropertyName("category_type")] public string? CategoryType { get; set; }
    [JsonPropertyName("action")] public string? Action { get; set; }
    [JsonPropertyName("operation_detail")] public string? OperationDetail { get; set; }
}

/// <summary>GET /report/remote_support.</summary>
public class RemoteSupportReportResult
{
    [JsonPropertyName("page_size")] public int PageSize { get; set; }
    [JsonPropertyName("next_page_token")] public string? NextPageToken { get; set; }
    [JsonPropertyName("remote_support_logs")] public List<RemoteSupportLogEntry>? RemoteSupportLogs { get; set; }
}

public class RemoteSupportLogEntry
{
    [JsonPropertyName("meeting_uuid")] public string? MeetingUuid { get; set; }
    [JsonPropertyName("meeting_number")] public string? MeetingNumber { get; set; }
    [JsonPropertyName("topic")] public string? Topic { get; set; }
    [JsonPropertyName("meeting_start_time")] public DateTimeOffset? MeetingStartTime { get; set; }
    [JsonPropertyName("meeting_host_id")] public string? MeetingHostId { get; set; }
    [JsonPropertyName("supporter_name")] public string? SupporterName { get; set; }
    [JsonPropertyName("supporter_email")] public string? SupporterEmail { get; set; }
    [JsonPropertyName("supportee_name")] public string? SupporteeName { get; set; }
    [JsonPropertyName("supportee_email")] public string? SupporteeEmail { get; set; }
    [JsonPropertyName("request_time")] public DateTimeOffset? RequestTime { get; set; }
    [JsonPropertyName("wait_time")] public int? WaitTime { get; set; }
    [JsonPropertyName("start_time")] public DateTimeOffset? StartTime { get; set; }
    [JsonPropertyName("end_time")] public DateTimeOffset? EndTime { get; set; }
    [JsonPropertyName("duration")] public int? Duration { get; set; }
}

/// <summary>GET /report/telephone.</summary>
public class TelephoneReportResult
{
    [JsonPropertyName("from")] public string? From { get; set; }
    [JsonPropertyName("to")] public string? To { get; set; }
    [JsonPropertyName("page_count")] public int PageCount { get; set; }
    [JsonPropertyName("page_size")] public int PageSize { get; set; }
    [JsonPropertyName("total_records")] public int TotalRecords { get; set; }
    [JsonPropertyName("next_page_token")] public string? NextPageToken { get; set; }
    [JsonPropertyName("telephony_usage")] public List<TelephoneUsageEntry>? TelephonyUsage { get; set; }
}

public class TelephoneUsageEntry
{
    [JsonPropertyName("uuid")] public string? Uuid { get; set; }
    [JsonPropertyName("meeting_id")] public string? MeetingId { get; set; }
    [JsonPropertyName("meeting_type")] public string? MeetingType { get; set; }
    [JsonPropertyName("host_id")] public string? HostId { get; set; }
    [JsonPropertyName("host_name")] public string? HostName { get; set; }
    [JsonPropertyName("host_email")] public string? HostEmail { get; set; }
    [JsonPropertyName("dept")] public string? Dept { get; set; }
    [JsonPropertyName("call_in_number")] public string? CallInNumber { get; set; }
    [JsonPropertyName("country_name")] public string? CountryName { get; set; }
    [JsonPropertyName("phone_number")] public string? PhoneNumber { get; set; }
    [JsonPropertyName("signaled_number")] public string? SignaledNumber { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("start_time")] public DateTimeOffset? StartTime { get; set; }
    [JsonPropertyName("end_time")] public DateTimeOffset? EndTime { get; set; }
    [JsonPropertyName("duration")] public int? Duration { get; set; }
    [JsonPropertyName("total")] public decimal? Total { get; set; }
    [JsonPropertyName("rate")] public decimal? Rate { get; set; }
}

/// <summary>GET /report/upcoming_events.</summary>
public class UpcomingEventsReportResult
{
    [JsonPropertyName("from")] public string? From { get; set; }
    [JsonPropertyName("to")] public string? To { get; set; }
    [JsonPropertyName("page_size")] public int PageSize { get; set; }
    [JsonPropertyName("next_page_token")] public string? NextPageToken { get; set; }
    [JsonPropertyName("upcoming_events")] public List<UpcomingEventEntry>? UpcomingEvents { get; set; }
}

public class UpcomingEventEntry
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("topic")] public string? Topic { get; set; }
    [JsonPropertyName("host_id")] public string? HostId { get; set; }
    [JsonPropertyName("host_name")] public string? HostName { get; set; }
    [JsonPropertyName("dept")] public string? Dept { get; set; }
    [JsonPropertyName("start_time")] public DateTimeOffset? StartTime { get; set; }
}

/// <summary>GET /report/users.</summary>
public class UsersReportResult
{
    [JsonPropertyName("from")] public string? From { get; set; }
    [JsonPropertyName("to")] public string? To { get; set; }
    [JsonPropertyName("page_count")] public int PageCount { get; set; }
    [JsonPropertyName("page_number")] public int PageNumber { get; set; }
    [JsonPropertyName("page_size")] public int PageSize { get; set; }
    [JsonPropertyName("total_records")] public int TotalRecords { get; set; }
    [JsonPropertyName("next_page_token")] public string? NextPageToken { get; set; }
    [JsonPropertyName("total_meetings")] public int? TotalMeetings { get; set; }
    [JsonPropertyName("total_participants")] public int? TotalParticipants { get; set; }
    [JsonPropertyName("total_meeting_minutes")] public int? TotalMeetingMinutes { get; set; }
    [JsonPropertyName("users")] public List<UserReportEntry>? Users { get; set; }
}

public class UserReportEntry
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("user_name")] public string? UserName { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("type")] public int? Type { get; set; }
    [JsonPropertyName("dept")] public string? Dept { get; set; }
    [JsonPropertyName("meetings")] public int? Meetings { get; set; }
    [JsonPropertyName("participants")] public int? Participants { get; set; }
    [JsonPropertyName("meeting_minutes")] public int? MeetingMinutes { get; set; }
    [JsonExtensionData] public Dictionary<string, JsonElement>? ExtraData { get; set; }
}

/// <summary>GET /report/users/{userId}/meetings.</summary>
public class UserMeetingsReportResult
{
    [JsonPropertyName("from")] public string? From { get; set; }
    [JsonPropertyName("to")] public string? To { get; set; }
    [JsonPropertyName("page_count")] public int PageCount { get; set; }
    [JsonPropertyName("page_number")] public int PageNumber { get; set; }
    [JsonPropertyName("page_size")] public int PageSize { get; set; }
    [JsonPropertyName("total_records")] public int TotalRecords { get; set; }
    [JsonPropertyName("next_page_token")] public string? NextPageToken { get; set; }
    [JsonPropertyName("meetings")] public List<UserMeetingReportEntry>? Meetings { get; set; }
}

public class UserMeetingReportEntry
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("uuid")] public string? Uuid { get; set; }
    [JsonPropertyName("topic")] public string? Topic { get; set; }
    [JsonPropertyName("type")] public int? Type { get; set; }
    [JsonPropertyName("user_email")] public string? UserEmail { get; set; }
    [JsonPropertyName("user_name")] public string? UserName { get; set; }
    [JsonPropertyName("start_time")] public DateTimeOffset? StartTime { get; set; }
    [JsonPropertyName("end_time")] public DateTimeOffset? EndTime { get; set; }
    [JsonPropertyName("duration")] public int? Duration { get; set; }
    [JsonPropertyName("total_minutes")] public int? TotalMinutes { get; set; }
    [JsonPropertyName("participants_count")] public int? ParticipantsCount { get; set; }
    [JsonPropertyName("has_recording")] public bool? HasRecording { get; set; }
    [JsonPropertyName("has_chat")] public bool? HasChat { get; set; }
    [JsonExtensionData] public Dictionary<string, JsonElement>? ExtraData { get; set; }
}

/// <summary>GET /report/webinars/{webinarId}/participants.</summary>
public class WebinarReportParticipantsResult
{
    [JsonPropertyName("page_count")] public int PageCount { get; set; }
    [JsonPropertyName("page_size")] public int PageSize { get; set; }
    [JsonPropertyName("total_records")] public int TotalRecords { get; set; }
    [JsonPropertyName("next_page_token")] public string? NextPageToken { get; set; }
    [JsonPropertyName("participants")] public List<WebinarReportParticipant>? Participants { get; set; }
}

public class WebinarReportParticipant
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("user_id")] public string? UserId { get; set; }
    [JsonPropertyName("registrant_id")] public string? RegistrantId { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("user_email")] public string? UserEmail { get; set; }
    [JsonPropertyName("join_time")] public DateTimeOffset? JoinTime { get; set; }
    [JsonPropertyName("leave_time")] public DateTimeOffset? LeaveTime { get; set; }
    [JsonPropertyName("duration")] public int? Duration { get; set; }
    [JsonPropertyName("status")] public string? Status { get; set; }
    [JsonExtensionData] public Dictionary<string, JsonElement>? ExtraData { get; set; }
}
