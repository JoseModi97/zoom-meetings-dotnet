using System.Net.Http;
using System.Text.Json;
using ZoomMeetings;
using ZoomMeetings.Models;

Console.WriteLine("====================================================");
Console.WriteLine("ZoomMeetings Console Script");
Console.WriteLine("====================================================");

// Read from environment variables, with fallback placeholders.
var config = new ZoomConfig
{
    AccountId = Environment.GetEnvironmentVariable("ZOOM_ACCOUNT_ID") ?? "YOUR_ACCOUNT_ID",
    ClientId = Environment.GetEnvironmentVariable("ZOOM_CLIENT_ID") ?? "YOUR_CLIENT_ID",
    ClientSecret = Environment.GetEnvironmentVariable("ZOOM_CLIENT_SECRET") ?? "YOUR_CLIENT_SECRET",
};

using var client = new ZoomClient(config);

// 1. Pagination: walk every page of upcoming meetings via EnumerateMeetingsAsync.
Console.WriteLine("\nUpcoming meetings:");
await foreach (var meeting in client.EnumerateMeetingsAsync("me"))
{
    Console.WriteLine($"  [{meeting.Id}] {meeting.Topic} - {meeting.StartTime}");
}

// 2. Typed create.
Console.WriteLine("\nCreating a test meeting...");
var created = await client.CreateMeetingAsync("me", new CreateMeetingRequest
{
    Topic = $"Console script test - {DateTimeOffset.UtcNow:u}",
    StartTime = DateTimeOffset.UtcNow.AddDays(1),
    Duration = 30,
});
Console.WriteLine($"  Created meeting {created?.Id}: {created?.JoinUrl}");

// 3. The raw escape hatch: any Zoom Meetings API operation, typed or not.
Console.WriteLine("\nRaw escape hatch example (GET /users/me):");
var me = await client.CallAsync<JsonElement>(HttpMethod.Get, "/users/me");
Console.WriteLine($"  Signed in as: {me.GetProperty("email").GetString()}");
