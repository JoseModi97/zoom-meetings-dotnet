using ZoomMeetings;
using ZoomMeetings.Examples.AllEndpoints.Domains;

Console.WriteLine("==================================================================");
Console.WriteLine("ZoomMeetings .NET SDK - Complete 186-Endpoints Example Suite");
Console.WriteLine("==================================================================");

// Load credentials from environment or use placeholders
var config = new ZoomConfig
{
    AccountId = Environment.GetEnvironmentVariable("ZOOM_ACCOUNT_ID") ?? "fET3dezQTN6iSY83c6bjNQ",
    ClientId = Environment.GetEnvironmentVariable("ZOOM_CLIENT_ID") ?? "lfYSqrmpQWKkKi_H4mJdTg",
    ClientSecret = Environment.GetEnvironmentVariable("ZOOM_CLIENT_SECRET") ?? "mEhT95S1PoROoG66k0pG4BiKGtZgvOfR",
};

using var client = new ZoomClient(config);

Console.WriteLine($"Running all 186 endpoints across 13 domains against Zoom API...\n");

var startTime = DateTime.UtcNow;

// 1. Admin & Common Domains (42 ops: Templates, SIP Phones, Live Controls, Summaries, Tracking Fields, TSP, Polls, Registrants)
await AdminDomainsExamples.RunAsync(client);

// 2. Devices, ZPA, and H.323 (17 ops)
await DevicesH323Examples.RunAsync(client);

// 3. Cloud Recordings & Archiving (23 ops)
await RecordingsArchivingExamples.RunAsync(client);

// 4. Reports (24 ops)
await ReportsExamples.RunAsync(client);

// 5. Meetings Core (27 ops)
await MeetingsCoreExamples.RunAsync(client);

// 6. Webinars (53 ops)
await WebinarsExamples.RunAsync(client);

var elapsed = DateTime.UtcNow - startTime;

Console.WriteLine("\n==================================================================");
Console.WriteLine($"Execution completed in {elapsed.TotalSeconds:F1}s.");
Console.WriteLine("All 186 operations demonstrated successfully.");
Console.WriteLine("==================================================================");
