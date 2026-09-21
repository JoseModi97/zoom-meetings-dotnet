using System.Text.Json;
using ZoomMeetings;
using ZoomMeetings.AspNetCore;
using ZoomMeetings.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Register ZoomMeetings in Dependency Injection (binds the "Zoom" appsettings.json section).
builder.Services.AddZoomMeetings(builder.Configuration);

var app = builder.Build();

// 2. List a user's upcoming meetings.
app.MapGet("/meetings", async (ZoomClient client) =>
{
    var meetings = await client.ListUpcomingMeetingsAsync("me");
    return Results.Ok(meetings);
});

// 3. Create a meeting.
app.MapPost("/meetings", async (ZoomClient client, CreateMeetingRequest request) =>
{
    var meeting = await client.CreateMeetingAsync("me", request);
    return Results.Created($"/meetings/{meeting?.Id}", meeting);
});

// 4. Zoom event webhook - signature verification and the endpoint.url_validation handshake are
//    handled automatically; this callback only runs for events that pass verification.
var webhookSecret = builder.Configuration["Zoom:WebhookSecretToken"] ?? "YOUR_WEBHOOK_SECRET_TOKEN";
app.MapZoomWebhook("/webhooks/zoom", webhookSecret, async (JsonElement payload, HttpContext ctx) =>
{
    var eventType = payload.TryGetProperty("event", out var eventProp) ? eventProp.GetString() : "(unknown)";
    Console.WriteLine($"[Zoom webhook] {eventType}");
    // TODO: react to meeting.started, meeting.participant_joined, etc.
    await Task.CompletedTask;
});

app.Run();
