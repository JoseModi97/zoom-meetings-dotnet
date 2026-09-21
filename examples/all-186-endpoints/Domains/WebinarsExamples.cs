using ZoomMeetings.Models;

namespace ZoomMeetings.Examples.AllEndpoints.Domains;

public static class WebinarsExamples
{
    public static async Task RunAsync(ZoomClient client, string dummyWebinarId = "98765432101")
    {
        Console.WriteLine("\n--- Webinars (53 ops) ---");
        await DomainRunner.RunOperationAsync("webinars", "GET /users/{userId}/webinars", async () =>
            await client.ListWebinarsAsync("me"));
        await DomainRunner.RunOperationAsync("webinarCreate", "POST /users/{userId}/webinars", async () =>
            await client.CreateWebinarAsync("me", new CreateWebinarRequest { Topic = "Example Webinar", Duration = 60, StartTime = DateTimeOffset.UtcNow.AddDays(2) }));
        await DomainRunner.RunOperationAsync("listWebinarTemplates", "GET /users/{userId}/webinar_templates", async () =>
            await client.ListWebinarTemplatesAsync("me"));
        await DomainRunner.RunOperationAsync("webinarTemplateCreate", "POST /users/{userId}/webinar_templates", async () =>
            await client.CreateWebinarTemplateAsync("me", new CreateWebinarTemplateRequest { WebinarId = 98765432101, Name = "Webinar Tpl" }));
        await DomainRunner.RunOperationAsync("webinar", "GET /webinars/{webinarId}", async () =>
            await client.GetWebinarAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("webinarUpdate", "PATCH /webinars/{webinarId}", async () =>
            await client.UpdateWebinarAsync(dummyWebinarId, new UpdateWebinarRequest { Topic = "Updated Webinar" }));
        await DomainRunner.RunOperationAsync("webinarDelete", "DELETE /webinars/{webinarId}", async () =>
            await client.DeleteWebinarAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("webinarStatus", "PUT /webinars/{webinarId}/status", async () =>
            await client.UpdateWebinarStatusAsync(dummyWebinarId));

        // Live webinar chat
        await DomainRunner.RunOperationAsync("deleteWebinarChatMessageById", "DELETE /live_webinars/{webinarId}/chat/messages/{messageId}", async () =>
            await client.DeleteLiveWebinarChatMessageAsync(dummyWebinarId, "msg_123"));

        // Past webinars
        await DomainRunner.RunOperationAsync("pastWebinars", "GET /past_webinars/{webinarId}/instances", async () =>
            await client.ListPastWebinarInstancesAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("listWebinarParticipants", "GET /past_webinars/{webinarId}/participants", async () =>
            await client.ListPastWebinarParticipantsAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("webinarAbsentees", "GET /past_webinars/{webinarId}/absentees", async () =>
            await client.ListPastWebinarAbsenteesAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("listPastWebinarPollResults", "GET /past_webinars/{webinarId}/polls", async () =>
            await client.ListPastWebinarPollResultsAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("listPastWebinarQA", "GET /past_webinars/{webinarId}/qa", async () =>
            await client.ListPastWebinarQaAsync(dummyWebinarId));

        // Registrants
        await DomainRunner.RunOperationAsync("webinarRegistrants", "GET /webinars/{webinarId}/registrants", async () =>
            await client.ListWebinarRegistrantsAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("webinarRegistrantCreate", "POST /webinars/{webinarId}/registrants", async () =>
            await client.AddWebinarRegistrantAsync(dummyWebinarId, new AddRegistrantRequest { Email = "webinarreg@example.com", FirstName = "John" }));
        await DomainRunner.RunOperationAsync("addBatchWebinarRegistrants", "POST /webinars/{webinarId}/batch_registrants", async () =>
            await client.AddBatchWebinarRegistrantsAsync(dummyWebinarId, new AddBatchRegistrantsRequest { Registrants = new() { new() { Email = "wbatch@example.com", FirstName = "Jane" } } }));
        await DomainRunner.RunOperationAsync("webinarRegistrantGet", "GET /webinars/{webinarId}/registrants/{registrantId}", async () =>
            await client.GetWebinarRegistrantAsync(dummyWebinarId, "wreg_123"));
        await DomainRunner.RunOperationAsync("deleteWebinarRegistrant", "DELETE /webinars/{webinarId}/registrants/{registrantId}", async () =>
            await client.DeleteWebinarRegistrantAsync(dummyWebinarId, "wreg_123"));
        await DomainRunner.RunOperationAsync("webinarRegistrantStatus", "PUT /webinars/{webinarId}/registrants/status", async () =>
            await client.UpdateWebinarRegistrantStatusAsync(dummyWebinarId, "approve", new[] { "wreg_123" }));
        await DomainRunner.RunOperationAsync("webinarRegistrantsQuestionsGet", "GET /webinars/{webinarId}/registrants/questions", async () =>
            await client.GetWebinarRegistrationQuestionsAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("webinarRegistrantQuestionUpdate", "PATCH /webinars/{webinarId}/registrants/questions", async () =>
            await client.UpdateWebinarRegistrationQuestionsAsync(dummyWebinarId, new RegistrationQuestions()));

        // Panelists
        await DomainRunner.RunOperationAsync("webinarPanelists", "GET /webinars/{webinarId}/panelists", async () =>
            await client.ListWebinarPanelistsAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("webinarPanelistCreate", "POST /webinars/{webinarId}/panelists", async () =>
            await client.AddWebinarPanelistsAsync(dummyWebinarId, new AddPanelistsRequest { Panelists = new() { new() { Email = "panelist@example.com", Name = "Dr. Panelist" } } }));
        await DomainRunner.RunOperationAsync("webinarPanelistDelete", "DELETE /webinars/{webinarId}/panelists/{panelistId}", async () =>
            await client.RemoveWebinarPanelistAsync(dummyWebinarId, "pan_123"));
        await DomainRunner.RunOperationAsync("webinarPanelistsDelete", "DELETE /webinars/{webinarId}/panelists", async () =>
            await client.RemoveAllWebinarPanelistsAsync(dummyWebinarId));

        // Polls
        await DomainRunner.RunOperationAsync("webinarPolls", "GET /webinars/{webinarId}/polls", async () =>
            await client.ListWebinarPollsAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("webinarPollCreate", "POST /webinars/{webinarId}/polls", async () =>
            await client.CreateWebinarPollAsync(dummyWebinarId, new Poll { Title = "Webinar Poll", Questions = new() { new() { Name = "WQ1", Type = "single", Answers = new() { "A", "B" } } } }));
        await DomainRunner.RunOperationAsync("webinarPollGet", "GET /webinars/{webinarId}/polls/{pollId}", async () =>
            await client.GetWebinarPollAsync(dummyWebinarId, "wpoll_123"));
        await DomainRunner.RunOperationAsync("webinarPollUpdate", "PUT /webinars/{webinarId}/polls/{pollId}", async () =>
            await client.UpdateWebinarPollAsync(dummyWebinarId, "wpoll_123", new Poll { Title = "Updated WPoll" }));
        await DomainRunner.RunOperationAsync("webinarPollDelete", "DELETE /webinars/{webinarId}/polls/{pollId}", async () =>
            await client.DeleteWebinarPollAsync(dummyWebinarId, "wpoll_123"));

        // Branding
        await DomainRunner.RunOperationAsync("getWebinarBranding", "GET /webinars/{webinarId}/branding", async () =>
            await client.GetWebinarBrandingAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("createWebinarBrandingNameTag", "POST /webinars/{webinarId}/branding/name_tags", async () =>
            await client.CreateWebinarBrandingNameTagAsync(dummyWebinarId, new NameTagRequest { Name = "Keynote Speaker" }));
        await DomainRunner.RunOperationAsync("updateWebinarBrandingNameTag", "PATCH /webinars/{webinarId}/branding/name_tags/{nameTagId}", async () =>
            await client.UpdateWebinarBrandingNameTagAsync(dummyWebinarId, "tag_123", new NameTagRequest { Name = "Updated Speaker" }));
        await DomainRunner.RunOperationAsync("deleteWebinarBrandingNameTag", "DELETE /webinars/{webinarId}/branding/name_tags", async () =>
            await client.DeleteWebinarBrandingNameTagsAsync(dummyWebinarId, new[] { "tag_123" }));
        await DomainRunner.RunOperationAsync("uploadWebinarBrandingVB", "POST /webinars/{webinarId}/branding/virtual_backgrounds", async () =>
        {
            using var ms = new MemoryStream(new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 });
            await client.UploadWebinarBrandingVirtualBackgroundAsync(dummyWebinarId, ms, "bg.jpg");
        });
        await DomainRunner.RunOperationAsync("setWebinarBrandingVB", "PATCH /webinars/{webinarId}/branding/virtual_backgrounds", async () =>
            await client.SetDefaultWebinarBrandingVirtualBackgroundAsync(dummyWebinarId, "vb_123"));
        await DomainRunner.RunOperationAsync("deleteWebinarBrandingVB", "DELETE /webinars/{webinarId}/branding/virtual_backgrounds", async () =>
            await client.DeleteWebinarBrandingVirtualBackgroundsAsync(dummyWebinarId, new[] { "vb_123" }));
        await DomainRunner.RunOperationAsync("uploadWebinarBrandingWallpaper", "POST /webinars/{webinarId}/branding/wallpaper", async () =>
        {
            using var ms = new MemoryStream(new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 });
            await client.UploadWebinarBrandingWallpaperAsync(dummyWebinarId, ms, "wp.jpg");
        });
        await DomainRunner.RunOperationAsync("deleteWebinarBrandingWallpaper", "DELETE /webinars/{webinarId}/branding/wallpaper", async () =>
            await client.DeleteWebinarBrandingWallpaperAsync(dummyWebinarId));

        // Connectors / Tokens / Links
        await DomainRunner.RunOperationAsync("webinarInviteLinksCreate", "POST /webinars/{webinarId}/invite_links", async () =>
            await client.CreateWebinarInviteLinksAsync(dummyWebinarId, new CreateInviteLinksRequest { Attendees = new() { new() { Name = "Guest" } } }));
        await DomainRunner.RunOperationAsync("webinarLiveStreamingJoinToken", "GET /webinars/{webinarId}/jointoken/live_streaming", async () =>
            await client.GetWebinarLiveStreamingJoinTokenAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("webinarLocalArchivingArchiveToken", "GET /webinars/{webinarId}/jointoken/local_archiving", async () =>
            await client.GetWebinarLocalArchivingTokenAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("webinarLocalRecordingJoinToken", "GET /webinars/{webinarId}/jointoken/local_recording", async () =>
            await client.GetWebinarLocalRecordingJoinTokenAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("getWebinarLiveStreamDetails", "GET /webinars/{webinarId}/livestream", async () =>
            await client.GetWebinarLiveStreamDetailsAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("webinarLiveStreamUpdate", "PATCH /webinars/{webinarId}/livestream", async () =>
            await client.UpdateWebinarLiveStreamAsync(dummyWebinarId, new UpdateLiveStreamRequest { StreamUrl = "rtmp://webinar.example.com", StreamKey = "wkey123" }));
        await DomainRunner.RunOperationAsync("webinarLiveStreamStatusUpdate", "PATCH /webinars/{webinarId}/livestream/status", async () =>
            await client.UpdateWebinarLiveStreamStatusAsync(dummyWebinarId, new UpdateLiveStreamStatusRequest { Action = "stop" }));
        await DomainRunner.RunOperationAsync("getWebinarSipDialingWithPasscode", "POST /webinars/{webinarId}/sip_dialing", async () =>
            await client.GetWebinarSipDialingAsync(dummyWebinarId, passcode: "654321"));
        await DomainRunner.RunOperationAsync("webinarSurveyGet", "GET /webinars/{webinarId}/survey", async () =>
            await client.GetWebinarSurveyAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("webinarSurveyUpdate", "PATCH /webinars/{webinarId}/survey", async () =>
            await client.UpdateWebinarSurveyAsync(dummyWebinarId, new MeetingSurvey()));
        await DomainRunner.RunOperationAsync("webinarSurveyDelete", "DELETE /webinars/{webinarId}/survey", async () =>
            await client.DeleteWebinarSurveyAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("webinarToken", "GET /webinars/{webinarId}/token", async () =>
            await client.GetWebinarTokenAsync(dummyWebinarId));
        await DomainRunner.RunOperationAsync("getTrackingSources", "GET /webinars/{webinarId}/tracking_sources", async () =>
            await client.GetWebinarTrackingSourcesAsync(dummyWebinarId));
    }
}
