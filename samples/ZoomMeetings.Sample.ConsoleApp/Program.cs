using ZoomMeetings;

// Reads ZOOM_ACCOUNT_ID / ZOOM_CLIENT_ID / ZOOM_CLIENT_SECRET from the environment (see README for how
// to create a Server-to-Server OAuth app in the Zoom Marketplace and get these three values).
var config = new ZoomConfig();

if (string.IsNullOrWhiteSpace(config.AccountId) || string.IsNullOrWhiteSpace(config.ClientId) || string.IsNullOrWhiteSpace(config.ClientSecret))
{
    Console.WriteLine("Set ZOOM_ACCOUNT_ID, ZOOM_CLIENT_ID, and ZOOM_CLIENT_SECRET environment variables first.");
    return 1;
}

using var client = new ZoomClient(config);

Console.WriteLine("Fetching your upcoming meetings...");
var upcoming = await client.ListUpcomingMeetingsAsync("me");

if (upcoming.Count == 0)
{
    Console.WriteLine("No upcoming meetings found.");
}
else
{
    foreach (var meeting in upcoming)
    {
        Console.WriteLine($"- [{meeting.Id}] {meeting.Topic} at {meeting.StartTime}");
    }
}

Console.WriteLine();
Console.WriteLine("Creating a test meeting...");
var created = await client.CreateMeetingAsync("me", new ZoomMeetings.Models.CreateMeetingRequest
{
    Topic = "ZoomMeetings sample - test meeting",
    Type = 2,
    StartTime = DateTimeOffset.UtcNow.AddDays(1),
    Duration = 30,
});

Console.WriteLine($"Created meeting {created?.Id}: {created?.JoinUrl}");

return 0;
