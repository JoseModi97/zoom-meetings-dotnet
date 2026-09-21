namespace ZoomMeetings.Examples.AllEndpoints.Domains;

public static class ReportsExamples
{
    public static async Task RunAsync(ZoomClient client, string dummyMeetingId = "12345678901", string dummyWebinarId = "98765432101")
    {
        var from = DateTimeOffset.UtcNow.AddDays(-7);
        var to = DateTimeOffset.UtcNow;

        Console.WriteLine("\n--- Reports (24 ops) ---");
        await DomainRunner.RunOperationAsync("reportSignInSignOutActivities", "GET /report/activities", async () =>
            await client.GetSignInSignOutActivityReportAsync(from: from, to: to));
        await DomainRunner.RunOperationAsync("getBillingReport", "GET /report/billing", async () =>
            await client.GetBillingReportAsync());
        await DomainRunner.RunOperationAsync("getBillingInvoicesReports", "GET /report/billing/invoices", async () =>
            await client.GetBillingInvoicesReportAsync("billing_123"));
        await DomainRunner.RunOperationAsync("reportCloudRecording", "GET /report/cloud_recording", async () =>
            await client.GetCloudRecordingUsageReportAsync(from: from, to: to));
        await DomainRunner.RunOperationAsync("reportDaily", "GET /report/daily", async () =>
            await client.GetDailyUsageReportAsync(year: DateTime.UtcNow.Year, month: DateTime.UtcNow.Month));
        await DomainRunner.RunOperationAsync("Getdisclaimerreport", "GET /report/disclaimer", async () =>
            await client.GetDisclaimerReportAsync(from: from, to: to));
        await DomainRunner.RunOperationAsync("Gethistorymeetingandwebinarlist", "GET /report/history_meetings", async () =>
            await client.GetHistoryMeetingsReportAsync(from: from, to: to));
        await DomainRunner.RunOperationAsync("reportMeetingactivitylogs", "GET /report/meeting_activities", async () =>
            await client.GetMeetingActivitiesReportAsync(from, to, activityType: "all"));
        await DomainRunner.RunOperationAsync("reportMeetingDetails", "GET /report/meetings/{meetingId}", async () =>
            await client.GetMeetingReportDetailAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("reportMeetingParticipants", "GET /report/meetings/{meetingId}/participants", async () =>
            await client.GetMeetingReportParticipantsAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("reportMeetingPolls", "GET /report/meetings/{meetingId}/polls", async () =>
            await client.GetMeetingPollReportAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("reportMeetingQA", "GET /report/meetings/{meetingId}/qa", async () =>
            await client.GetMeetingQaReportAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("reportMeetingSurvey", "GET /report/meetings/{meetingId}/survey", async () =>
            await client.GetMeetingSurveyReportAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("reportOperationLogs", "GET /report/operationlogs", async () =>
            await client.GetOperationLogsReportAsync(categoryType: "all", from: from, to: to));
        await DomainRunner.RunOperationAsync("Getremotesupportreport", "GET /report/remote_support", async () =>
            await client.GetRemoteSupportReportAsync(from: from, to: to));
        await DomainRunner.RunOperationAsync("reportTelephone", "GET /report/telephone", async () =>
            await client.GetTelephoneReportAsync(from: from, to: to));
        await DomainRunner.RunOperationAsync("reportUpcomingEvents", "GET /report/upcoming_events", async () =>
            await client.GetUpcomingEventsReportAsync(from: from, to: to));
        await DomainRunner.RunOperationAsync("reportUsers", "GET /report/users", async () =>
            await client.GetUsersReportAsync(from: from, to: to));
        await DomainRunner.RunOperationAsync("reportMeetings", "GET /report/users/{userId}/meetings", async () =>
            await client.GetUserMeetingsReportAsync("me", from: from, to: to));
        await DomainRunner.RunOperationAsync("reportWebinarDetails", "GET /report/webinars/{webinarId}", async () =>
            await client.GetWebinarReportDetailAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("reportWebinarParticipants", "GET /report/webinars/{webinarId}/participants", async () =>
            await client.GetWebinarReportParticipantsAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("reportWebinarPolls", "GET /report/webinars/{webinarId}/polls", async () =>
            await client.GetWebinarPollReportAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("reportWebinarQA", "GET /report/webinars/{webinarId}/qa", async () =>
            await client.GetWebinarQaReportAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("reportWebinarSurvey", "GET /report/webinars/{webinarId}/survey", async () =>
            await client.GetWebinarSurveyReportAsync(dummyWebinarId));
    }
}
