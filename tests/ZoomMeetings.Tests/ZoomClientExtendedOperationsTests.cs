using System.Net;
using System.Net.Http;
using ZoomMeetings.Models;
using Xunit;

namespace ZoomMeetings.Tests;

public class ZoomClientExtendedOperationsTests
{
    private static ZoomConfig ValidConfig() => new()
    {
        AccountId = "acct-1",
        ClientId = "client-1",
        ClientSecret = "secret-1",
    };

    private static HttpResponseMessage TokenResponse() => new(HttpStatusCode.OK)
    {
        Content = new StringContent("""{"access_token":"mock-token","expires_in":3600}"""),
    };

    [Fact]
    public async Task MeetingSummaries_UserSummaries_HitsCorrectEndpoint()
    {
        HttpRequestMessage? captured = null;
        var handler = new TestHttpMessageHandler(req =>
        {
            if (req.RequestUri!.Host == "zoom.us") return TokenResponse();
            captured = req;
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"page_size":30,"summaries":[]}"""),
            };
        });

        using var client = new ZoomClient(ValidConfig(), handler);
        var res = await client.ListUserMeetingSummariesAsync("user123", pageSize: 30);

        Assert.NotNull(res);
        Assert.Equal(HttpMethod.Get, captured!.Method);
        Assert.Contains("/users/user123/meeting_summaries", captured.RequestUri!.PathAndQuery);
        Assert.Contains("page_size=30", captured.RequestUri.Query);
    }

    [Fact]
    public async Task Polls_BatchAndPast_HitCorrectEndpoints()
    {
        var paths = new List<string>();
        var handler = new TestHttpMessageHandler(req =>
        {
            if (req.RequestUri!.Host == "zoom.us") return TokenResponse();
            paths.Add(req.RequestUri!.PathAndQuery);
            if (req.Method == HttpMethod.Post)
            {
                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("""{"polls":[{"id":"p1","title":"Batch Poll"}]}"""),
                };
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"id":12345,"questions":[]}"""),
            };
        });

        using var client = new ZoomClient(ValidConfig(), handler);
        var batchRes = await client.CreateBatchPollsAsync("12345", new CreateBatchPollsRequest
        {
            Polls = new List<Poll> { new() { Title = "Batch Poll" } }
        });
        var pastRes = await client.GetPastMeetingPollsAsync("12345");

        Assert.NotNull(batchRes);
        Assert.NotNull(pastRes);
        Assert.Contains("/meetings/12345/batch_polls", paths[0]);
        Assert.Contains("/past_meetings/12345/polls", paths[1]);
    }

    [Fact]
    public async Task Registrants_BatchAndQuestions_HitCorrectEndpoints()
    {
        var paths = new List<string>();
        var handler = new TestHttpMessageHandler(req =>
        {
            if (req.RequestUri!.Host == "zoom.us") return TokenResponse();
            paths.Add(req.RequestUri!.PathAndQuery);
            if (req.Method == HttpMethod.Post)
            {
                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("""{"registrants":[{"registrant_id":"r1","email":"test@example.com"}]}"""),
                };
            }
            if (req.Method == HttpMethod.Get)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"questions":[],"custom_questions":[]}"""),
                };
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        });

        using var client = new ZoomClient(ValidConfig(), handler);
        var batchRes = await client.AddBatchRegistrantsAsync("12345", new AddBatchRegistrantsRequest
        {
            Registrants = new List<BatchRegistrantRequest> { new() { Email = "test@example.com", FirstName = "Test" } }
        });
        var qRes = await client.GetRegistrationQuestionsAsync("12345");
        await client.UpdateRegistrationQuestionsAsync("12345", new RegistrationQuestions());

        Assert.NotNull(batchRes);
        Assert.NotNull(qRes);
        Assert.Contains("/meetings/12345/batch_registrants", paths[0]);
        Assert.Contains("/meetings/12345/registrants/questions", paths[1]);
        Assert.Contains("/meetings/12345/registrants/questions", paths[2]);
    }

    [Fact]
    public async Task Templates_ListAndCreate_HitCorrectEndpoints()
    {
        var paths = new List<string>();
        var handler = new TestHttpMessageHandler(req =>
        {
            if (req.RequestUri!.Host == "zoom.us") return TokenResponse();
            paths.Add(req.RequestUri!.PathAndQuery);
            if (req.Method == HttpMethod.Post)
            {
                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("""{"id":"tpl1","name":"Template 1"}"""),
                };
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"total_records":1,"templates":[{"id":"tpl1","name":"Template 1"}]}"""),
            };
        });

        using var client = new ZoomClient(ValidConfig(), handler);
        var list = await client.ListMeetingTemplatesAsync("me");
        var created = await client.CreateMeetingTemplateAsync("me", new CreateMeetingTemplateRequest
        {
            MeetingId = "12345",
            Name = "Template 1"
        });

        Assert.NotNull(list);
        Assert.NotNull(created);
        Assert.Contains("/users/me/meeting_templates", paths[0]);
        Assert.Contains("/users/me/meeting_templates", paths[1]);
    }

    [Fact]
    public async Task LiveMeetings_ChatAndEvents_HitCorrectEndpoints()
    {
        var methods = new List<HttpMethod>();
        var paths = new List<string>();
        var handler = new TestHttpMessageHandler(req =>
        {
            if (req.RequestUri!.Host == "zoom.us") return TokenResponse();
            methods.Add(req.Method);
            paths.Add(req.RequestUri!.PathAndQuery);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        });

        using var client = new ZoomClient(ValidConfig(), handler);
        await client.DeleteLiveMeetingChatMessageAsync("m1", "msg1", "f1,f2");
        await client.UpdateLiveMeetingChatMessageAsync("m1", "msg1", "updated text");
        await client.InMeetingControlAsync("m1", new InMeetingControlRequest { Method = "participant.remove" });
        await client.UpdateMeetingRtmsStatusAsync("m1", new MeetingRtmsStatusUpdateRequest { Action = "start" });

        Assert.Equal(HttpMethod.Delete, methods[0]);
        Assert.Contains("/live_meetings/m1/chat/messages/msg1", paths[0]);
        Assert.Contains("file_ids=f1%2Cf2", paths[0]);

        Assert.Equal("PATCH", methods[1].Method);
        Assert.Contains("/live_meetings/m1/chat/messages/msg1", paths[1]);

        Assert.Equal("PATCH", methods[2].Method);
        Assert.Contains("/live_meetings/m1/events", paths[2]);

        Assert.Equal("PATCH", methods[3].Method);
        Assert.Contains("/live_meetings/m1/rtms_app/status", paths[3]);
    }

    [Fact]
    public async Task TrackingFields_Crud_HitsCorrectEndpoints()
    {
        var paths = new List<string>();
        var handler = new TestHttpMessageHandler(req =>
        {
            if (req.RequestUri!.Host == "zoom.us") return TokenResponse();
            paths.Add(req.RequestUri!.PathAndQuery);
            if (req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath == "/v2/tracking_fields")
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"total_records":1,"tracking_fields":[{"id":"tf1","field":"Cost Center"}]}"""),
                };
            }
            if (req.Method == HttpMethod.Post)
            {
                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("""{"id":"tf1","field":"Cost Center"}"""),
                };
            }
            if (req.Method == HttpMethod.Get)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"id":"tf1","field":"Cost Center"}"""),
                };
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        });

        using var client = new ZoomClient(ValidConfig(), handler);
        var list = await client.ListTrackingFieldsAsync();
        var created = await client.CreateTrackingFieldAsync(new CreateTrackingFieldRequest { Field = "Cost Center" });
        var get = await client.GetTrackingFieldAsync("tf1");
        await client.UpdateTrackingFieldAsync("tf1", new UpdateTrackingFieldRequest { Field = "Updated" });
        await client.DeleteTrackingFieldAsync("tf1");

        Assert.NotNull(list);
        Assert.NotNull(created);
        Assert.NotNull(get);
        Assert.Equal(5, paths.Count);
    }

    [Fact]
    public async Task SipPhones_Crud_HitsCorrectEndpoints()
    {
        var paths = new List<string>();
        var handler = new TestHttpMessageHandler(req =>
        {
            if (req.RequestUri!.Host == "zoom.us") return TokenResponse();
            paths.Add(req.RequestUri!.PathAndQuery);
            if (req.Method == HttpMethod.Get)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"page_size":30,"phones":[{"phone_id":"sp1"}]}"""),
                };
            }
            if (req.Method == HttpMethod.Post)
            {
                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("""{"phone_id":"sp1","domain":"sip.example.com"}"""),
                };
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        });

        using var client = new ZoomClient(ValidConfig(), handler);
        var list = await client.ListSipPhonesAsync(searchKey: "test");
        var enabled = await client.EnableSipPhoneAsync(new EnableSipPhoneRequest { Domain = "sip.example.com" });
        await client.UpdateSipPhoneAsync("sp1", new UpdateSipPhoneRequest { Domain = "sip2.example.com" });
        await client.DeleteSipPhoneAsync("sp1");

        Assert.NotNull(list);
        Assert.NotNull(enabled);
        Assert.Contains("search_key=test", paths[0]);
        Assert.Equal(4, paths.Count);
    }

    [Fact]
    public async Task Tsp_Operations_HitCorrectEndpoints()
    {
        var paths = new List<string>();
        var handler = new TestHttpMessageHandler(req =>
        {
            if (req.RequestUri!.Host == "zoom.us") return TokenResponse();
            paths.Add(req.RequestUri!.PathAndQuery);
            if (req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath == "/v2/tsp")
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"tsp_enabled":true}"""),
                };
            }
            if (req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath.EndsWith("/tsp"))
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"tsp_accounts":[]}"""),
                };
            }
            if (req.Method == HttpMethod.Post)
            {
                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("""{"id":"tsp1","conference_code":"123"}"""),
                };
            }
            if (req.Method == HttpMethod.Get)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"id":"tsp1","conference_code":"123"}"""),
                };
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        });

        using var client = new ZoomClient(ValidConfig(), handler);
        var acctTsp = await client.GetAccountTspAsync();
        await client.UpdateAccountTspAsync(new UpdateAccountTspSettingsRequest { TspEnabled = true });
        var userTsps = await client.ListUserTspsAsync("u1");
        var created = await client.CreateUserTspAsync("u1", new CreateUserTspRequest { ConferenceCode = "123", LeaderPin = "456" });
        await client.UpdateUserTspUrlAsync("u1", "https://tsp.example.com");
        var userTsp = await client.GetUserTspAsync("u1", "tsp1");
        await client.UpdateUserTspAsync("u1", "tsp1", new UpdateUserTspRequest { ConferenceCode = "999" });
        await client.DeleteUserTspAsync("u1", "tsp1");

        Assert.NotNull(acctTsp);
        Assert.NotNull(userTsps);
        Assert.NotNull(created);
        Assert.NotNull(userTsp);
        Assert.Equal(8, paths.Count);
    }

    [Fact]
    public async Task Devices_And_H323_HitCorrectEndpoints()
    {
        var paths = new List<string>();
        var handler = new TestHttpMessageHandler(req =>
        {
            if (req.RequestUri!.Host == "zoom.us") return TokenResponse();
            paths.Add(req.RequestUri!.PathAndQuery);
            if (req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath == "/v2/devices")
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"page_size":30,"devices":[{"device_id":"d1"}]}"""),
                };
            }
            if (req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath == "/v2/devices/groups")
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"page_size":30,"groups":[{"zdm_group_id":"g1"}]}"""),
                };
            }
            if (req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath == "/v2/devices/zpa/settings")
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"language":"en","timezone":"UTC"}"""),
                };
            }
            if (req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath.Contains("/devices/zpa/zdm_groups/"))
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"firmware_versions":[],"app_versions":[]}"""),
                };
            }
            if (req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath.StartsWith("/v2/devices/"))
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"device_id":"d1","device_name":"Room A"}"""),
                };
            }
            if (req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath == "/v2/h323/devices")
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"page_size":30,"devices":[{"id":"h1","name":"H323 Unit"}]}"""),
                };
            }
            if (req.Method == HttpMethod.Post && req.RequestUri.AbsolutePath == "/v2/h323/devices")
            {
                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("""{"id":"h1","name":"H323 Unit"}"""),
                };
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        });

        using var client = new ZoomClient(ValidConfig(), handler);

        // Devices (8)
        var devices = await client.ListDevicesAsync();
        await client.CreateDeviceAsync(new AddDeviceRequest { DeviceName = "Test" });
        var device = await client.GetDeviceAsync("d1");
        await client.UpdateDeviceAsync("d1", new UpdateDeviceRequest { DeviceName = "Updated" });
        await client.DeleteDeviceAsync("d1");
        await client.AssignDeviceGroupAsync("d1", "grp1");
        await client.ChangeDeviceAssociationAsync("d1", new ChangeDeviceAssociationRequest { RoomId = "r1" });
        var groups = await client.ListDeviceGroupsAsync();

        // ZPA (5)
        await client.AssignZpaDeviceAsync(new ZpaAssignmentRequest { MacAddress = "00:11:22:33:44:55", Vendor = "poly" });
        var zpaSettings = await client.GetZpaUserSettingsAsync("u1");
        await client.UpgradeZpaAsync(new ZpaUpgradeRequest { ZdmGroupId = "g1" });
        await client.DeleteZpaDeviceAsync("poly", "00:11:22:33:44:55");
        var zpaVersions = await client.GetZpaVersionsAsync("g1");

        // H.323 (4)
        var h323List = await client.ListH323DevicesAsync();
        var h323Created = await client.CreateH323DeviceAsync(new CreateH323DeviceRequest { Name = "H323 Unit", Ip = "1.2.3.4", Protocol = "H.323", Encryption = "auto" });
        await client.DeleteH323DeviceAsync("h1");
        await client.UpdateH323DeviceAsync("h1", new UpdateH323DeviceRequest { Name = "H323 Updated" });

        Assert.NotNull(devices);
        Assert.NotNull(device);
        Assert.NotNull(groups);
        Assert.NotNull(zpaSettings);
        Assert.NotNull(zpaVersions);
        Assert.NotNull(h323List);
        Assert.NotNull(h323Created);
        Assert.Equal(17, paths.Count);
    }
}
