using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>GET /report/meetings/{meetingId} - meeting detail report (only available after the meeting ends).</summary>
    public Task<MeetingReportDetail?> GetMeetingReportDetailAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<MeetingReportDetail>(HttpMethod.Get, $"/report/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}", cancellationToken: cancellationToken);

    /// <summary>GET /report/meetings/{meetingId}/participants - meeting participants report.</summary>
    public Task<ListMeetingReportParticipantsResult?> GetMeetingReportParticipantsAsync(
        string meetingId,
        int? pageSize = null,
        string? nextPageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;

        return CallAsync<ListMeetingReportParticipantsResult>(HttpMethod.Get, $"/report/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/participants", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/activities - sign-in/sign-out activity report.</summary>
    public Task<SignInSignOutActivityReportResult?> GetSignInSignOutActivityReportAsync(
        DateTimeOffset? from = null, DateTimeOffset? to = null, int? pageSize = null, string? nextPageToken = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (from != null) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to != null) query["to"] = to.Value.ToString("yyyy-MM-dd");
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        return CallAsync<SignInSignOutActivityReportResult>(HttpMethod.Get, "/report/activities", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/billing - account billing report.</summary>
    public Task<BillingReportResult?> GetBillingReportAsync(CancellationToken cancellationToken = default)
        => CallAsync<BillingReportResult>(HttpMethod.Get, "/report/billing", cancellationToken: cancellationToken);

    /// <summary>GET /report/billing/invoices - billing invoice report for a given billing ID.</summary>
    public Task<BillingInvoicesReportResult?> GetBillingInvoicesReportAsync(string billingId, CancellationToken cancellationToken = default)
        => CallAsync<BillingInvoicesReportResult>(HttpMethod.Get, "/report/billing/invoices", query: new Dictionary<string, string?> { ["billing_id"] = billingId }, cancellationToken: cancellationToken);

    /// <summary>GET /report/cloud_recording - cloud recording storage usage report.</summary>
    public Task<CloudRecordingUsageReportResult?> GetCloudRecordingUsageReportAsync(DateTimeOffset from, DateTimeOffset to, string? groupId = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["from"] = from.ToString("yyyy-MM-dd"), ["to"] = to.ToString("yyyy-MM-dd") };
        if (groupId != null) query["group_id"] = groupId;
        return CallAsync<CloudRecordingUsageReportResult>(HttpMethod.Get, "/report/cloud_recording", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/daily - daily usage report for a given month.</summary>
    public Task<DailyUsageReportResult?> GetDailyUsageReportAsync(int? year = null, int? month = null, string? groupId = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (year != null) query["year"] = year.Value.ToString();
        if (month != null) query["month"] = month.Value.ToString();
        if (groupId != null) query["group_id"] = groupId;
        return CallAsync<DailyUsageReportResult>(HttpMethod.Get, "/report/daily", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/disclaimer - meeting/webinar disclaimer report.</summary>
    public Task<DisclaimerReportResult?> GetDisclaimerReportAsync(
        DateTimeOffset from, DateTimeOffset to, string? searchValue = null, string? disclaimerType = null, string? groupId = null,
        int? pageSize = null, string? nextPageToken = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["from"] = from.ToString("yyyy-MM-dd"), ["to"] = to.ToString("yyyy-MM-dd") };
        if (searchValue != null) query["search_value"] = searchValue;
        if (disclaimerType != null) query["disclaimer_type"] = disclaimerType;
        if (groupId != null) query["group_id"] = groupId;
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        return CallAsync<DisclaimerReportResult>(HttpMethod.Get, "/report/disclaimer", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/history_meetings - history of meetings and webinars held in a date range.</summary>
    public Task<HistoryMeetingsReportResult?> GetHistoryMeetingsReportAsync(
        DateTimeOffset from, DateTimeOffset to, string? dateType = null, string? meetingType = null, string? reportType = null, string? searchKey = null,
        int? pageSize = null, string? nextPageToken = null, string? groupId = null, string? meetingFeature = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["from"] = from.ToString("yyyy-MM-dd"), ["to"] = to.ToString("yyyy-MM-dd") };
        if (dateType != null) query["date_type"] = dateType;
        if (meetingType != null) query["meeting_type"] = meetingType;
        if (reportType != null) query["report_type"] = reportType;
        if (searchKey != null) query["search_key"] = searchKey;
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (groupId != null) query["group_id"] = groupId;
        if (meetingFeature != null) query["meeting_feature"] = meetingFeature;
        return CallAsync<HistoryMeetingsReportResult>(HttpMethod.Get, "/report/history_meetings", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/meeting_activities - a meeting's activity log report.</summary>
    public Task<MeetingActivitiesReportResult?> GetMeetingActivitiesReportAsync(
        DateTimeOffset from, DateTimeOffset to, string activityType, int? pageSize = null, string? nextPageToken = null, string? meetingNumber = null, string? searchKey = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["from"] = from.ToString("yyyy-MM-dd"), ["to"] = to.ToString("yyyy-MM-dd"), ["activity_type"] = activityType };
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (meetingNumber != null) query["meeting_number"] = meetingNumber;
        if (searchKey != null) query["search_key"] = searchKey;
        return CallAsync<MeetingActivitiesReportResult>(HttpMethod.Get, "/report/meeting_activities", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/meetings/{meetingId}/polls - meeting poll report.</summary>
    public Task<PollReportResult?> GetMeetingPollReportAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<PollReportResult>(HttpMethod.Get, $"/report/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/polls", cancellationToken: cancellationToken);

    /// <summary>GET /report/meetings/{meetingId}/qa - meeting Q&amp;A report.</summary>
    public Task<QaResult?> GetMeetingQaReportAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<QaResult>(HttpMethod.Get, $"/report/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/qa", cancellationToken: cancellationToken);

    /// <summary>GET /report/meetings/{meetingId}/survey - meeting survey report.</summary>
    public Task<SurveyReportResult?> GetMeetingSurveyReportAsync(string meetingId, CancellationToken cancellationToken = default)
        => CallAsync<SurveyReportResult>(HttpMethod.Get, $"/report/meetings/{ZoomIdEncoding.EncodePathSegment(meetingId)}/survey", cancellationToken: cancellationToken);

    /// <summary>GET /report/operationlogs - account operation logs report.</summary>
    public Task<OperationLogsReportResult?> GetOperationLogsReportAsync(
        DateTimeOffset from, DateTimeOffset to, int? pageSize = null, string? nextPageToken = null, string? categoryType = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["from"] = from.ToString("yyyy-MM-dd"), ["to"] = to.ToString("yyyy-MM-dd") };
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (categoryType != null) query["category_type"] = categoryType;
        return CallAsync<OperationLogsReportResult>(HttpMethod.Get, "/report/operationlogs", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/remote_support - remote support session report.</summary>
    public Task<RemoteSupportReportResult?> GetRemoteSupportReportAsync(DateTimeOffset from, DateTimeOffset to, string? nextPageToken = null, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["from"] = from.ToString("yyyy-MM-dd"), ["to"] = to.ToString("yyyy-MM-dd") };
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        return CallAsync<RemoteSupportReportResult>(HttpMethod.Get, "/report/remote_support", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/telephone - telephone (PSTN) usage report.</summary>
    public Task<TelephoneReportResult?> GetTelephoneReportAsync(
        DateTimeOffset from, DateTimeOffset to, string? type = null, string? queryDateType = null, int? pageSize = null, int? pageNumber = null, string? nextPageToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["from"] = from.ToString("yyyy-MM-dd"), ["to"] = to.ToString("yyyy-MM-dd") };
        if (type != null) query["type"] = type;
        if (queryDateType != null) query["query_date_type"] = queryDateType;
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (pageNumber != null) query["page_number"] = pageNumber.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        return CallAsync<TelephoneReportResult>(HttpMethod.Get, "/report/telephone", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/upcoming_events - upcoming meetings/webinars report.</summary>
    public Task<UpcomingEventsReportResult?> GetUpcomingEventsReportAsync(
        DateTimeOffset from, DateTimeOffset to, int? pageSize = null, string? nextPageToken = null, string? type = null, string? groupId = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["from"] = from.ToString("yyyy-MM-dd"), ["to"] = to.ToString("yyyy-MM-dd") };
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (type != null) query["type"] = type;
        if (groupId != null) query["group_id"] = groupId;
        return CallAsync<UpcomingEventsReportResult>(HttpMethod.Get, "/report/upcoming_events", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/users - active/inactive host report.</summary>
    public Task<UsersReportResult?> GetUsersReportAsync(
        DateTimeOffset from, DateTimeOffset to, string? type = null, int? pageSize = null, int? pageNumber = null, string? nextPageToken = null, string? groupId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["from"] = from.ToString("yyyy-MM-dd"), ["to"] = to.ToString("yyyy-MM-dd") };
        if (type != null) query["type"] = type;
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (pageNumber != null) query["page_number"] = pageNumber.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (groupId != null) query["group_id"] = groupId;
        return CallAsync<UsersReportResult>(HttpMethod.Get, "/report/users", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/users/{userId}/meetings - a user's meeting report.</summary>
    public Task<UserMeetingsReportResult?> GetUserMeetingsReportAsync(
        string userId, DateTimeOffset from, DateTimeOffset to, int? pageSize = null, string? nextPageToken = null, string? type = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?> { ["from"] = from.ToString("yyyy-MM-dd"), ["to"] = to.ToString("yyyy-MM-dd") };
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (type != null) query["type"] = type;
        return CallAsync<UserMeetingsReportResult>(HttpMethod.Get, $"/report/users/{Uri.EscapeDataString(userId)}/meetings", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/webinars/{webinarId} - webinar detail report.</summary>
    public Task<MeetingReportDetail?> GetWebinarReportDetailAsync(string webinarId, CancellationToken cancellationToken = default)
        => CallAsync<MeetingReportDetail>(HttpMethod.Get, $"/report/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}", cancellationToken: cancellationToken);

    /// <summary>GET /report/webinars/{webinarId}/participants - webinar participants report.</summary>
    public Task<WebinarReportParticipantsResult?> GetWebinarReportParticipantsAsync(
        string webinarId, int? pageSize = null, string? nextPageToken = null, string? includeFields = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (pageSize != null) query["page_size"] = pageSize.Value.ToString();
        if (nextPageToken != null) query["next_page_token"] = nextPageToken;
        if (includeFields != null) query["include_fields"] = includeFields;
        return CallAsync<WebinarReportParticipantsResult>(HttpMethod.Get, $"/report/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/participants", query: query, cancellationToken: cancellationToken);
    }

    /// <summary>GET /report/webinars/{webinarId}/polls - webinar poll report.</summary>
    public Task<PollReportResult?> GetWebinarPollReportAsync(string webinarId, CancellationToken cancellationToken = default)
        => CallAsync<PollReportResult>(HttpMethod.Get, $"/report/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/polls", cancellationToken: cancellationToken);

    /// <summary>GET /report/webinars/{webinarId}/qa - webinar Q&amp;A report.</summary>
    public Task<QaResult?> GetWebinarQaReportAsync(string webinarId, CancellationToken cancellationToken = default)
        => CallAsync<QaResult>(HttpMethod.Get, $"/report/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/qa", cancellationToken: cancellationToken);

    /// <summary>GET /report/webinars/{webinarId}/survey - webinar survey report.</summary>
    public Task<SurveyReportResult?> GetWebinarSurveyReportAsync(string webinarId, CancellationToken cancellationToken = default)
        => CallAsync<SurveyReportResult>(HttpMethod.Get, $"/report/webinars/{ZoomIdEncoding.EncodePathSegment(webinarId)}/survey", cancellationToken: cancellationToken);
}
