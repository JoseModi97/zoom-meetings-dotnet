using ZoomMeetings.Models;

namespace ZoomMeetings.Examples.AllEndpoints.Domains;

public static class MeetingsCoreExamples
{
    public static async Task RunAsync(ZoomClient client, string dummyMeetingId = "12345678901")
    {
        Console.WriteLine("\n--- Meetings Core (27 ops) ---");
        await DomainRunner.RunOperationAsync("meetings", "GET /users/{userId}/meetings", async () =>
            await client.ListMeetingsAsync("me"));
        await DomainRunner.RunOperationAsync("listUpcomingMeeting", "GET /users/{userId}/upcoming_meetings", async () =>
            await client.ListUpcomingMeetingsAsync("me"));
        await DomainRunner.RunOperationAsync("userPACs", "GET /users/{userId}/pac", async () =>
            await client.ListUserPacAccountsAsync("me"));
        await DomainRunner.RunOperationAsync("meetingCreate", "POST /users/{userId}/meetings", async () =>
            await client.CreateMeetingAsync("me", new CreateMeetingRequest { Topic = "Sample Core Meeting", Duration = 30, StartTime = DateTimeOffset.UtcNow.AddDays(1) }));
        await DomainRunner.RunOperationAsync("meeting", "GET /meetings/{meetingId}", async () =>
            await client.GetMeetingAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("meetingUpdate", "PATCH /meetings/{meetingId}", async () =>
            await client.UpdateMeetingAsync(dummyMeetingId, new UpdateMeetingRequest { Topic = "Updated Sample Core Meeting" }));
        await DomainRunner.RunOperationAsync("meetingDelete", "DELETE /meetings/{meetingId}", async () =>
            await client.DeleteMeetingAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("meetingStatus", "PUT /meetings/{meetingId}/status", async () =>
            await client.UpdateMeetingStatusAsync(dummyMeetingId, "end"));
        await DomainRunner.RunOperationAsync("meetingInvitation", "GET /meetings/{meetingId}/invitation", async () =>
            await client.GetMeetingInvitationAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("meetingInviteLinksCreate", "POST /meetings/{meetingId}/invite_links", async () =>
            await client.CreateMeetingInviteLinksAsync(dummyMeetingId, new CreateInviteLinksRequest { Attendees = new() { new() { Name = "Sam" } }, Ttl = 7200 }));
        await DomainRunner.RunOperationAsync("meetingLiveStreamingJoinToken", "GET /meetings/{meetingId}/jointoken/live_streaming", async () =>
            await client.GetMeetingLiveStreamingJoinTokenAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("meetingLocalArchivingArchiveToken", "GET /meetings/{meetingId}/jointoken/local_archiving", async () =>
            await client.GetMeetingLocalArchivingTokenAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("meetingLocalRecordingJoinToken", "GET /meetings/{meetingId}/jointoken/local_recording", async () =>
            await client.GetMeetingLocalRecordingJoinTokenAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("getMeetingLiveStreamDetails", "GET /meetings/{meetingId}/livestream", async () =>
            await client.GetMeetingLiveStreamDetailsAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("meetingLiveStreamUpdate", "PATCH /meetings/{meetingId}/livestream", async () =>
            await client.UpdateMeetingLiveStreamAsync(dummyMeetingId, new UpdateLiveStreamRequest { StreamUrl = "rtmp://live.example.com", StreamKey = "key123" }));
        await DomainRunner.RunOperationAsync("meetingLiveStreamStatusUpdate", "PATCH /meetings/{meetingId}/livestream/status", async () =>
            await client.UpdateMeetingLiveStreamStatusAsync(dummyMeetingId, new UpdateLiveStreamStatusRequest { Action = "stop" }));
        await DomainRunner.RunOperationAsync("meetingAppAdd", "POST /meetings/{meetingId}/open_apps", async () =>
            await client.AddMeetingAppAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("meetingAppDelete", "DELETE /meetings/{meetingId}/open_apps", async () =>
            await client.DeleteMeetingAppAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("getSipDialingWithPasscode", "POST /meetings/{meetingId}/sip_dialing", async () =>
            await client.GetMeetingSipDialingAsync(dummyMeetingId, passcode: "123456"));
        await DomainRunner.RunOperationAsync("meetingSurveyGet", "GET /meetings/{meetingId}/survey", async () =>
            await client.GetMeetingSurveyAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("meetingSurveyUpdate", "PATCH /meetings/{meetingId}/survey", async () =>
            await client.UpdateMeetingSurveyAsync(dummyMeetingId, new MeetingSurvey()));
        await DomainRunner.RunOperationAsync("meetingSurveyDelete", "DELETE /meetings/{meetingId}/survey", async () =>
            await client.DeleteMeetingSurveyAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("meetingToken", "GET /meetings/{meetingId}/token", async () =>
            await client.GetMeetingTokenAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("pastMeetingDetails", "GET /past_meetings/{meetingId}", async () =>
            await client.GetPastMeetingDetailsAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("pastMeetings", "GET /past_meetings/{meetingId}/instances", async () =>
            await client.ListPastMeetingInstancesAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("pastMeetingParticipants", "GET /past_meetings/{meetingId}/participants", async () =>
            await client.ListPastMeetingParticipantsAsync(dummyMeetingId));
        await DomainRunner.RunOperationAsync("listPastMeetingQA", "GET /past_meetings/{meetingId}/qa", async () =>
            await client.ListPastMeetingQaAsync(dummyMeetingId));
    }
}
