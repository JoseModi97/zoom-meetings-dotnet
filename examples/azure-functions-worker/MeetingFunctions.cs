using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using ZoomMeetings;
using ZoomMeetings.Models;

namespace AzureFunctionsExample;

public class MeetingFunctions
{
    private readonly ZoomClient _client;

    public MeetingFunctions()
    {
        // Azure Functions reads settings from Environment Variables / Application Settings
        // (local.settings.json's "Values" section when running locally).
        _client = new ZoomClient(new ZoomConfig
        {
            AccountId = Environment.GetEnvironmentVariable("ZOOM_ACCOUNT_ID") ?? "YOUR_ACCOUNT_ID",
            ClientId = Environment.GetEnvironmentVariable("ZOOM_CLIENT_ID") ?? "YOUR_CLIENT_ID",
            ClientSecret = Environment.GetEnvironmentVariable("ZOOM_CLIENT_SECRET") ?? "YOUR_CLIENT_SECRET",
        });
    }

    [Function("ListMeetings")]
    public async Task<HttpResponseData> ListMeetings(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "meetings")] HttpRequestData req)
    {
        var meetings = await _client.ListUpcomingMeetingsAsync("me");

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(meetings);
        return response;
    }

    [Function("CreateMeeting")]
    public async Task<HttpResponseData> CreateMeeting(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "meetings")] HttpRequestData req)
    {
        using var reader = new StreamReader(req.Body);
        var body = await reader.ReadToEndAsync();
        var request = JsonSerializer.Deserialize<CreateMeetingRequest>(body)
            ?? new CreateMeetingRequest { Topic = "New meeting" };

        var meeting = await _client.CreateMeetingAsync("me", request);

        var response = req.CreateResponse(HttpStatusCode.Created);
        await response.WriteAsJsonAsync(meeting);
        return response;
    }
}
