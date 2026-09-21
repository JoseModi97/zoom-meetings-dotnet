using ZoomMeetings.Models;

namespace ZoomMeetings.Examples.AllEndpoints.Domains;

public static class RecordingsArchivingExamples
{
    public static async Task RunAsync(ZoomClient client, string dummyMeetingId = "12345678901", string dummyFileId = "file_123")
    {
        Console.WriteLine("\n--- Cloud Recordings (17 ops) ---");
        await DomainRunner.RunOperationAsync("recordingsList", "GET /users/{userId}/recordings", async () =>
            await client.ListUserRecordingsAsync("me"));
        await DomainRunner.RunOperationAsync("recordingGet", "GET /meetings/{meetingId}/recordings", async () =>
            await client.GetMeetingRecordingsAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("recordingDelete", "DELETE /meetings/{meetingId}/recordings", async () =>
            await client.DeleteMeetingRecordingsAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("recordingStatusUpdate", "PUT /meetings/{meetingUUID}/recordings/status", async () =>
            await client.RecoverMeetingRecordingsAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("recordingDeleteOne", "DELETE /meetings/{meetingId}/recordings/{recordingId}", async () =>
            await client.DeleteRecordingFileAsync(dummyMeetingId, dummyFileId));
        await DomainRunner.RunOperationAsync("recordingStatusUpdateOne", "PUT /meetings/{meetingId}/recordings/{recordingId}/status", async () =>
            await client.RecoverRecordingFileAsync(dummyMeetingId, dummyFileId));
        await DomainRunner.RunOperationAsync("recordingSettingUpdate", "GET /meetings/{meetingId}/recordings/settings", async () =>
            await client.GetRecordingSettingsAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("recordingSettingsUpdate", "PATCH /meetings/{meetingId}/recordings/settings", async () =>
            await client.UpdateRecordingSettingsAsync(dummyMeetingId, new RecordingSettings { ShareRecording = "publicly" }));
        await DomainRunner.RunOperationAsync("analytics_details", "GET /meetings/{meetingId}/recordings/analytics_details", async () =>
            await client.GetRecordingAnalyticsDetailsAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("analytics_summary", "GET /meetings/{meetingId}/recordings/analytics_summary", async () =>
            await client.GetRecordingAnalyticsSummaryAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("meetingRecordingRegistrants", "GET /meetings/{meetingId}/recordings/registrants", async () =>
            await client.ListRecordingRegistrantsAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("meetingRecordingRegistrantCreate", "POST /meetings/{meetingId}/recordings/registrants", async () =>
            await client.AddRecordingRegistrantAsync(dummyMeetingId, new AddRegistrantRequest { Email = "rec@example.com", FirstName = "Jane" }));
        await DomainRunner.RunOperationAsync("recordingRegistrantsQuestionsGet", "GET /meetings/{meetingId}/recordings/registrants/questions", async () =>
            await client.GetRecordingRegistrationQuestionsAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("recordingRegistrantQuestionUpdate", "PATCH /meetings/{meetingId}/recordings/registrants/questions", async () =>
            await client.UpdateRecordingRegistrationQuestionsAsync(dummyMeetingId, new RegistrationQuestions()));
        await DomainRunner.RunOperationAsync("meetingRecordingRegistrantStatus", "PUT /meetings/{meetingId}/recordings/registrants/status", async () =>
            await client.UpdateRecordingRegistrantStatusAsync(dummyMeetingId, "approve", new[] { "reg_123" }));
        await DomainRunner.RunOperationAsync("GetMeetingTranscript", "GET /meetings/{meetingId}/transcript", async () =>
            await client.GetMeetingTranscriptAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("DeleteMeetingTranscript", "DELETE /meetings/{meetingId}/transcript", async () =>
            await client.DeleteMeetingTranscriptAsync(dummyMeetingId));

        Console.WriteLine("\n--- Archiving (6 ops) ---");
        await DomainRunner.RunOperationAsync("listArchivedFiles", "GET /archive_files", async () =>
            await client.ListArchivedFilesAsync(pageSize: 10));
        await DomainRunner.RunOperationAsync("getArchivedFileStatistics", "GET /archive_files/statistics", async () =>
            await client.GetArchivedFileStatisticsAsync(from: DateTimeOffset.UtcNow.AddDays(-7), to: DateTimeOffset.UtcNow));
        await DomainRunner.RunOperationAsync("listArchiveFileDownloadAudit", "GET /archive_files/download_audit", async () =>
            await client.ListArchiveFileDownloadAuditAsync(from: DateTimeOffset.UtcNow.AddDays(-7), to: DateTimeOffset.UtcNow));
        await DomainRunner.RunOperationAsync("updateArchivedFile", "PATCH /archive_files/{fileId}", async () =>
            await client.UpdateArchivedFileAutoDeleteAsync(dummyFileId, autoDelete: false));
        await DomainRunner.RunOperationAsync("getArchivedFiles", "GET /past_meetings/{meetingUUID}/archive_files", async () =>
            await client.GetMeetingArchivedFilesAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("deleteArchivedFiles", "DELETE /past_meetings/{meetingUUID}/archive_files", async () =>
            await client.DeleteMeetingArchivedFilesAsync(dummyMeetingId));
    }
}
