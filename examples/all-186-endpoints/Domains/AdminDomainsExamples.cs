using ZoomMeetings.Models;

namespace ZoomMeetings.Examples.AllEndpoints.Domains;

public static class AdminDomainsExamples
{
    public static async Task RunAsync(ZoomClient client, string dummyId = "12345678901")
    {
        Console.WriteLine("\n--- Templates (2 ops) ---");
        await DomainRunner.RunOperationAsync("listMeetingTemplates", "GET /users/{userId}/meeting_templates", async () =>
            await client.ListMeetingTemplatesAsync("me"));
        await DomainRunner.RunOperationAsync("meetingTemplateCreate", "POST /users/{userId}/meeting_templates", async () =>
            await client.CreateMeetingTemplateAsync("me", new CreateMeetingTemplateRequest { MeetingId = dummyId, Name = "Template Example" }));

        Console.WriteLine("\n--- SIP Phones (4 ops) ---");
        await DomainRunner.RunOperationAsync("ListSIPPhonePhones", "GET /sip_phones/phones", async () =>
            await client.ListSipPhonesAsync(pageSize: 10));
        await DomainRunner.RunOperationAsync("EnableSIPPhonePhones", "POST /sip_phones/phones", async () =>
            await client.EnableSipPhoneAsync(new EnableSipPhoneRequest { AuthorizationName = "sipuser", Domain = "sip.example.com", Password = "secretPassword123!", UserEmail = "sip@example.com", UserName = "Sip User" }));
        await DomainRunner.RunOperationAsync("UpdateSIPPhonePhones", "PATCH /sip_phones/phones/{phoneId}", async () =>
            await client.UpdateSipPhoneAsync(dummyId, new UpdateSipPhoneRequest { Domain = "sip2.example.com" }));
        await DomainRunner.RunOperationAsync("deleteSIPPhonePhones", "DELETE /sip_phones/phones/{phoneId}", async () =>
            await client.DeleteSipPhoneAsync(dummyId));

        Console.WriteLine("\n--- Live Meeting Controls (4 ops) ---");
        await DomainRunner.RunOperationAsync("deleteMeetingChatMessageById", "DELETE /live_meetings/{meetingId}/chat/messages/{messageId}", async () =>
            await client.DeleteLiveMeetingChatMessageAsync(dummyId, "msg_123"));
        await DomainRunner.RunOperationAsync("updateMeetingChatMessageById", "PATCH /live_meetings/{meetingId}/chat/messages/{messageId}", async () =>
            await client.UpdateLiveMeetingChatMessageAsync(dummyId, "msg_123", "Updated message content"));
        await DomainRunner.RunOperationAsync("inMeetingControl", "PATCH /live_meetings/{meetingId}/events", async () =>
            await client.InMeetingControlAsync(dummyId, new InMeetingControlRequest { Method = "participant.remove", Params = new InMeetingControlParams { ParticipantUuid = "dummy-uuid" } }));
        await DomainRunner.RunOperationAsync("meetingRTMSStatusUpdate", "PATCH /live_meetings/{meetingId}/rtms_app/status", async () =>
            await client.UpdateMeetingRtmsStatusAsync(dummyId, new MeetingRtmsStatusUpdateRequest { Action = "start", Settings = new MeetingRtmsSettings { ClientId = "app-client-id" } }));

        Console.WriteLine("\n--- Meeting Summaries (4 ops) ---");
        await DomainRunner.RunOperationAsync("Listmeetingsummaries", "GET /meetings/meeting_summaries", async () =>
            await client.ListAccountMeetingSummariesAsync(pageSize: 10));
        await DomainRunner.RunOperationAsync("ListUserMeetingSummaries", "GET /users/{userId}/meeting_summaries", async () =>
            await client.ListUserMeetingSummariesAsync("me", pageSize: 10));
        await DomainRunner.RunOperationAsync("Getameetingsummary", "GET /meetings/{meetingId}/meeting_summary", async () =>
            await client.GetMeetingSummaryAsync(dummyId));
        await DomainRunner.RunOperationAsync("Deletemeetingorwebinarsummary", "DELETE /meetings/{meetingId}/meeting_summary", async () =>
            await client.DeleteMeetingSummaryAsync(dummyId));

        Console.WriteLine("\n--- Tracking Fields (5 ops) ---");
        await DomainRunner.RunOperationAsync("trackingfieldList", "GET /tracking_fields", async () =>
            await client.ListTrackingFieldsAsync());
        await DomainRunner.RunOperationAsync("trackingfieldCreate", "POST /tracking_fields", async () =>
            await client.CreateTrackingFieldAsync(new CreateTrackingFieldRequest { Field = "ProjectCode", Required = false, Visible = true }));
        await DomainRunner.RunOperationAsync("trackingfieldGet", "GET /tracking_fields/{fieldId}", async () =>
            await client.GetTrackingFieldAsync("tf_123"));
        await DomainRunner.RunOperationAsync("trackingfieldUpdate", "PATCH /tracking_fields/{fieldId}", async () =>
            await client.UpdateTrackingFieldAsync("tf_123", new UpdateTrackingFieldRequest { Field = "UpdatedCode" }));
        await DomainRunner.RunOperationAsync("trackingfieldDelete", "DELETE /tracking_fields/{fieldId}", async () =>
            await client.DeleteTrackingFieldAsync("tf_123"));

        Console.WriteLine("\n--- Telephony Service Provider (TSP) (8 ops) ---");
        await DomainRunner.RunOperationAsync("tsp", "GET /tsp", async () =>
            await client.GetAccountTspAsync());
        await DomainRunner.RunOperationAsync("tspUpdate", "PATCH /tsp", async () =>
            await client.UpdateAccountTspAsync(new UpdateAccountTspSettingsRequest { TspEnabled = true }));
        await DomainRunner.RunOperationAsync("userTSPs", "GET /users/{userId}/tsp", async () =>
            await client.ListUserTspsAsync("me"));
        await DomainRunner.RunOperationAsync("userTSPCreate", "POST /users/{userId}/tsp", async () =>
            await client.CreateUserTspAsync("me", new CreateUserTspRequest { ConferenceCode = "123456", LeaderPin = "654321" }));
        await DomainRunner.RunOperationAsync("tspUrlUpdate", "PATCH /users/{userId}/tsp/settings", async () =>
            await client.UpdateUserTspUrlAsync("me", "https://tsp.example.com/audio"));
        await DomainRunner.RunOperationAsync("userTSP", "GET /users/{userId}/tsp/{tspId}", async () =>
            await client.GetUserTspAsync("me", "tsp_123"));
        await DomainRunner.RunOperationAsync("userTSPUpdate", "PATCH /users/{userId}/tsp/{tspId}", async () =>
            await client.UpdateUserTspAsync("me", "tsp_123", new UpdateUserTspRequest { ConferenceCode = "987654" }));
        await DomainRunner.RunOperationAsync("userTSPDelete", "DELETE /users/{userId}/tsp/{tspId}", async () =>
            await client.DeleteUserTspAsync("me", "tsp_123"));

        Console.WriteLine("\n--- Polls (7 ops) ---");
        await DomainRunner.RunOperationAsync("meetingPolls", "GET /meetings/{meetingId}/polls", async () =>
            await client.ListPollsAsync(dummyId));
        await DomainRunner.RunOperationAsync("meetingPollCreate", "POST /meetings/{meetingId}/polls", async () =>
            await client.CreatePollAsync(dummyId, new Poll { Title = "Example Poll", Questions = new() { new() { Name = "Q1", Type = "single", Answers = new() { "Yes", "No" } } } }));
        await DomainRunner.RunOperationAsync("meetingPollGet", "GET /meetings/{meetingId}/polls/{pollId}", async () =>
            await client.GetPollAsync(dummyId, "poll_123"));
        await DomainRunner.RunOperationAsync("meetingPollUpdate", "PUT /meetings/{meetingId}/polls/{pollId}", async () =>
            await client.UpdatePollAsync(dummyId, "poll_123", new Poll { Title = "Updated Poll" }));
        await DomainRunner.RunOperationAsync("meetingPollDelete", "DELETE /meetings/{meetingId}/polls/{pollId}", async () =>
            await client.DeletePollAsync(dummyId, "poll_123"));
        await DomainRunner.RunOperationAsync("createBatchPolls", "POST /meetings/{meetingId}/batch_polls", async () =>
            await client.CreateBatchPollsAsync(dummyId, new CreateBatchPollsRequest { Polls = new() { new() { Title = "Batch Poll 1" } } }));
        await DomainRunner.RunOperationAsync("listPastMeetingPolls", "GET /past_meetings/{meetingId}/polls", async () =>
            await client.GetPastMeetingPollsAsync(dummyId));

        Console.WriteLine("\n--- Registrants (8 ops) ---");
        await DomainRunner.RunOperationAsync("meetingRegistrants", "GET /meetings/{meetingId}/registrants", async () =>
            await client.ListRegistrantsAsync(dummyId));
        await DomainRunner.RunOperationAsync("meetingRegistrantCreate", "POST /meetings/{meetingId}/registrants", async () =>
            await client.AddRegistrantAsync(dummyId, new AddRegistrantRequest { Email = "user@example.com", FirstName = "Alice", LastName = "Smith" }));
        await DomainRunner.RunOperationAsync("meetingRegistrantGet", "GET /meetings/{meetingId}/registrants/{registrantId}", async () =>
            await client.GetRegistrantAsync(dummyId, "reg_123"));
        await DomainRunner.RunOperationAsync("meetingregistrantdelete", "DELETE /meetings/{meetingId}/registrants/{registrantId}", async () =>
            await client.DeleteRegistrantAsync(dummyId, "reg_123"));
        await DomainRunner.RunOperationAsync("meetingRegistrantStatus", "PUT /meetings/{meetingId}/registrants/status", async () =>
            await client.UpdateRegistrantStatusAsync(dummyId, "approve", new[] { "reg_123" }));
        await DomainRunner.RunOperationAsync("addBatchRegistrants", "POST /meetings/{meetingId}/batch_registrants", async () =>
            await client.AddBatchRegistrantsAsync(dummyId, new AddBatchRegistrantsRequest { Registrants = new() { new() { Email = "batch@example.com", FirstName = "Bob" } } }));
        await DomainRunner.RunOperationAsync("meetingRegistrantsQuestionsGet", "GET /meetings/{meetingId}/registrants/questions", async () =>
            await client.GetRegistrationQuestionsAsync(dummyId));
        await DomainRunner.RunOperationAsync("meetingRegistrantQuestionUpdate", "PATCH /meetings/{meetingId}/registrants/questions", async () =>
            await client.UpdateRegistrationQuestionsAsync(dummyId, new RegistrationQuestions()));
    }
}
