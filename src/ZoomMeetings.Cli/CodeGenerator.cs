namespace ZoomMeetings.Cli;

public static class CodeGenerator
{
    public static string GenerateCode(PlatformType platform, string? targetNamespace = null)
    {
        var ns = targetNamespace ?? "YourApp";

        return platform switch
        {
            PlatformType.AspNetCoreMinimalApi => GenerateMinimalApi(ns),
            PlatformType.AspNetCoreMvc => GenerateMvc(ns),
            PlatformType.AspNetCoreBlazor => GenerateBlazor(),
            PlatformType.AzureFunctions => GenerateAzureFunctions(ns),
            PlatformType.DotNetWorker => GenerateWorkerService(ns),
            _ => GenerateConsoleQuickstart(ns)
        };
    }

    public static string GenerateShellScript(ShellType shell, string accountId, string clientId, string clientSecret, string? profile)
    {
        var isDefault = string.IsNullOrEmpty(profile) || string.Equals(profile, "production", StringComparison.OrdinalIgnoreCase);
        var prefix = isDefault ? "ZOOM" : $"ZOOM_{profile!.ToUpperInvariant()}";

        if (shell == ShellType.PowerShell)
        {
            return $@"# Zoom Meetings Environment Variables (PowerShell)
# Run with: . .\zoom-env.ps1
$env:{prefix}_ACCOUNT_ID = ""{accountId}""
$env:{prefix}_CLIENT_ID = ""{clientId}""
$env:{prefix}_CLIENT_SECRET = ""{clientSecret}""

Write-Host ""✓ Zoom environment variables configured for profile '{(isDefault ? "Production" : profile)}'"" -ForegroundColor Green
";
        }
        else
        {
            return $@"#!/usr/bin/env bash
# Zoom Meetings Environment Variables (Bash/Zsh)
# Run with: source ./zoom-env.sh
export {prefix}_ACCOUNT_ID=""{accountId}""
export {prefix}_CLIENT_ID=""{clientId}""
export {prefix}_CLIENT_SECRET=""{clientSecret}""

echo ""✓ Zoom environment variables configured for profile '{(isDefault ? "Production" : profile)}'""
";
        }
    }

    private static string GenerateMinimalApi(string ns) =>
$@"using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using ZoomMeetings;
using ZoomMeetings.AspNetCore;
using ZoomMeetings.Models;

namespace {ns};

/// <summary>
/// Zoom Meetings Minimal API endpoints and Webhook receiver.
/// Register in Program.cs with:
///   builder.Services.AddZoomMeetings(builder.Configuration);
///   app.MapZoomEndpoints(builder.Configuration);
/// </summary>
public static class ZoomEndpoints
{{
    public static IEndpointRouteBuilder MapZoomEndpoints(this IEndpointRouteBuilder endpoints, IConfiguration configuration)
    {{
        var group = endpoints.MapGroup(""/api/zoom"");

        // 1. List upcoming scheduled meetings
        group.MapGet(""/meetings"", async (ZoomClient client) =>
        {{
            var meetings = await client.ListUpcomingMeetingsAsync(""me"");
            return Results.Ok(meetings);
        }});

        // 2. Create a new Zoom meeting
        group.MapPost(""/meetings"", async (ZoomClient client, CreateMeetingRequest request) =>
        {{
            var meeting = await client.CreateMeetingAsync(""me"", request);
            return Results.Created($""/api/zoom/meetings/{{meeting?.Id}}"", meeting);
        }});

        // 3. Retrieve meeting details
        group.MapGet(""/meetings/{{meetingId}}"", async (ZoomClient client, string meetingId) =>
        {{
            var meeting = await client.GetMeetingAsync(meetingId);
            return meeting != null ? Results.Ok(meeting) : Results.NotFound();
        }});

        // 4. Retrieve AI Companion meeting summary
        group.MapGet(""/meetings/{{meetingId}}/summary"", async (ZoomClient client, string meetingId) =>
        {{
            var summary = await client.GetMeetingSummaryAsync(meetingId);
            return summary != null ? Results.Ok(summary) : Results.NotFound();
        }});

        // 5. Automated Zoom Webhook (satisfies CRC validation and verifies HMAC-SHA256 signature)
        var webhookSecret = configuration[""Zoom:WebhookSecretToken""] ?? string.Empty;
        endpoints.MapZoomWebhook(""/webhooks/zoom"", webhookSecret, async (JsonElement payload, HttpContext ctx) =>
        {{
            var eventType = payload.TryGetProperty(""event"", out var ev) ? ev.GetString() : ""(unknown)"";
            Console.WriteLine($""[Zoom Webhook Received] Event: {{eventType}}"");

            switch (eventType)
            {{
                case ""meeting.started"":
                    // Handle live meeting start
                    break;
                case ""recording.completed"":
                    // Handle cloud recording completion
                    break;
                case ""meeting.summary_completed"":
                    // Handle AI summary generation
                    break;
            }}

            await Task.CompletedTask;
        }});

        return endpoints;
    }}
}}
";

    private static string GenerateMvc(string ns) =>
$@"using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ZoomMeetings;
using ZoomMeetings.Models;

namespace {ns};

[ApiController]
[Route(""api/[controller]"")]
public class ZoomMeetingsController : ControllerBase
{{
    private readonly ZoomClient _zoomClient;

    public ZoomMeetingsController(ZoomClient zoomClient)
    {{
        _zoomClient = zoomClient;
    }}

    [HttpGet(""meetings"")]
    public async Task<IActionResult> ListMeetings()
    {{
        var meetings = await _zoomClient.ListUpcomingMeetingsAsync(""me"");
        return Ok(meetings);
    }}

    [HttpPost(""meetings"")]
    public async Task<IActionResult> CreateMeeting([FromBody] CreateMeetingRequest request)
    {{
        var meeting = await _zoomClient.CreateMeetingAsync(""me"", request);
        return CreatedAtAction(nameof(GetMeeting), new {{ meetingId = meeting?.Id }}, meeting);
    }}

    [HttpGet(""meetings/{{meetingId}}"")]
    public async Task<IActionResult> GetMeeting(string meetingId)
    {{
        var meeting = await _zoomClient.GetMeetingAsync(meetingId);
        return meeting != null ? Ok(meeting) : NotFound();
    }}
}}
";

    private static string GenerateBlazor() =>
@"@page ""/zoom""
@using ZoomMeetings
@using ZoomMeetings.Models
@inject ZoomClient Zoom

<div class=""container mt-4"">
    <h2>Zoom Meetings Dashboard</h2>
    <p class=""text-muted"">Direct integration with Zoom Meetings .NET Client.</p>

    <div class=""mb-3"">
        <button class=""btn btn-primary"" @onclick=""LoadMeetings"" disabled=""@_loading"">
            @(_loading ? ""Loading..."" : ""Refresh Meetings"")
        </button>
        <button class=""btn btn-success ms-2"" @onclick=""CreateQuickMeeting"" disabled=""@_creating"">
            @(_creating ? ""Creating..."" : ""Create Quick Meeting"")
        </button>
    </div>

    @if (!string.IsNullOrEmpty(_statusMessage))
    {
        <div class=""alert alert-info"">@_statusMessage</div>
    }

    @if (_meetings != null)
    {
        <table class=""table table-striped table-hover"">
            <thead>
                <tr>
                    <th>Topic</th>
                    <th>Meeting ID</th>
                    <th>Start Time</th>
                    <th>Duration</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var meeting in _meetings)
                {
                    <tr>
                        <td><strong>@meeting.Topic</strong></td>
                        <td>@meeting.Id</td>
                        <td>@(meeting.StartTime?.ToLocalTime().ToString(""g"") ?? ""Instant"")</td>
                        <td>@(meeting.Duration) mins</td>
                        <td>
                            @if (!string.IsNullOrEmpty(meeting.JoinUrl))
                            {
                                <a href=""@meeting.JoinUrl"" target=""_blank"" class=""btn btn-sm btn-outline-primary"">Join</a>
                            }
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    }
</div>

@code {
    private List<Meeting>? _meetings;
    private bool _loading;
    private bool _creating;
    private string? _statusMessage;

    protected override async Task OnInitializedAsync()
    {
        await LoadMeetings();
    }

    private async Task LoadMeetings()
    {
        _loading = true;
        _statusMessage = null;
        try
        {
            var res = await Zoom.ListUpcomingMeetingsAsync(""me"");
            _meetings = res?.Meetings;
        }
        catch (Exception ex)
        {
            _statusMessage = $""Error: {ex.Message}"";
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task CreateQuickMeeting()
    {
        _creating = true;
        _statusMessage = null;
        try
        {
            var created = await Zoom.CreateMeetingAsync(""me"", new CreateMeetingRequest
            {
                Topic = $""Sync {DateTime.Now:t}"",
                Duration = 30
            });
            _statusMessage = $""Created: {created?.Topic} (ID: {created?.Id})"";
            await LoadMeetings();
        }
        catch (Exception ex)
        {
            _statusMessage = $""Error: {ex.Message}"";
        }
        finally
        {
            _creating = false;
        }
    }
}
";

    private static string GenerateAzureFunctions(string ns) =>
$@"using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using ZoomMeetings;
using ZoomMeetings.Models;

namespace {ns};

public class ZoomMeetingFunctions
{{
    private readonly ZoomClient _zoomClient;

    public ZoomMeetingFunctions(ZoomClient zoomClient)
    {{
        _zoomClient = zoomClient;
    }}

    [Function(""ZoomListMeetings"")]
    public async Task<HttpResponseData> ListMeetings(
        [HttpTrigger(AuthorizationLevel.Function, ""get"", Route = ""zoom/meetings"")] HttpRequestData req)
    {{
        var meetings = await _zoomClient.ListUpcomingMeetingsAsync(""me"");
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(meetings);
        return response;
    }}

    [Function(""ZoomCreateMeeting"")]
    public async Task<HttpResponseData> CreateMeeting(
        [HttpTrigger(AuthorizationLevel.Function, ""post"", Route = ""zoom/meetings"")] HttpRequestData req)
    {{
        var request = await JsonSerializer.DeserializeAsync<CreateMeetingRequest>(req.Body);
        if (request == null)
        {{
            var bad = req.CreateResponse(HttpStatusCode.BadRequest);
            await bad.WriteStringAsync(""Invalid JSON body."");
            return bad;
        }}

        var meeting = await _zoomClient.CreateMeetingAsync(""me"", request);
        var response = req.CreateResponse(HttpStatusCode.Created);
        await response.WriteAsJsonAsync(meeting);
        return response;
    }}
}}
";

    private static string GenerateWorkerService(string ns) =>
$@"using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZoomMeetings;

namespace {ns};

public class ZoomWorker : BackgroundService
{{
    private readonly ILogger<ZoomWorker> _logger;
    private readonly ZoomClient _zoomClient;

    public ZoomWorker(ILogger<ZoomWorker> logger, ZoomClient zoomClient)
    {{
        _logger = logger;
        _zoomClient = zoomClient;
    }}

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {{
        _logger.LogInformation(""ZoomWorker background service running."");

        while (!stoppingToken.IsCancellationRequested)
        {{
            try
            {{
                // Example: check for newly generated AI Companion summaries
                var summaries = await _zoomClient.ListMeetingSummariesAsync(cancellationToken: stoppingToken);
                _logger.LogInformation(""Retrieved {{Count}} meeting summaries."", summaries?.Summaries?.Count ?? 0);
            }}
            catch (Exception ex)
            {{
                _logger.LogError(ex, ""Error executing Zoom polling task."");
            }}

            // Poll every 5 minutes
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }}
    }}
}}
";

    private static string GenerateConsoleQuickstart(string ns) =>
$@"using ZoomMeetings;
using ZoomMeetings.Models;

namespace {ns};

public static class ZoomQuickstart
{{
    public static async Task RunAsync()
    {{
        // Load credentials from environment or pass directly
        var config = new ZoomConfig
        {{
            AccountId = Environment.GetEnvironmentVariable(""ZOOM_ACCOUNT_ID"") ?? ""your_account_id"",
            ClientId = Environment.GetEnvironmentVariable(""ZOOM_CLIENT_ID"") ?? ""your_client_id"",
            ClientSecret = Environment.GetEnvironmentVariable(""ZOOM_CLIENT_SECRET"") ?? ""your_client_secret""
        }};

        using var client = new ZoomClient(config);

        Console.WriteLine(""Scheduling a Zoom meeting with 100% typed client..."");
        var meeting = await client.CreateMeetingAsync(""me"", new CreateMeetingRequest
        {{
            Topic = ""Automated Integration Meeting"",
            Duration = 30,
            Settings = new MeetingSettings
            {{
                WaitingRoom = true,
                AutoRecording = ""cloud""
            }}
        }});

        Console.WriteLine($""✓ Meeting created successfully!"");
        Console.WriteLine($""  ID       : {{meeting?.Id}}"");
        Console.WriteLine($""  Join URL : {{meeting?.JoinUrl}}"");
    }}
}}
";
}
