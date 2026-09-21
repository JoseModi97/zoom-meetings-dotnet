# ZoomMeetings — Complete Zoom Meetings API Client for .NET

Idiomatic, high-performance .NET / C# client library and CLI tool providing **100% typed coverage (all 186 operations)** for the [Zoom Meetings REST API](https://developers.zoom.us/docs/api/).

From scheduling meetings and moderating live sessions to fetching AI Companion summaries, orchestrating cloud recordings, producing branded webinars, and auditing enterprise compliance — every tool is fully typed, tested, and ready on day one.

> **Unofficial.** This project is not affiliated with, endorsed by, or sponsored by Zoom Video Communications, Inc. "Zoom" is a trademark of Zoom Video Communications, Inc.

---

## Packages

| Package | Description | Target frameworks |
|---|---|---|
| [`ZoomMeetings`](src/ZoomMeetings) | Core typed client (`ZoomClient`) with built-in OAuth2 & retry engine | `netstandard2.0`, `net8.0`, `net10.0` |
| [`ZoomMeetings.AspNetCore`](src/ZoomMeetings.AspNetCore) | ASP.NET Core DI registration (`AddZoomMeetings`) + webhook signature & CRC verification | `net8.0`, `net10.0` |
| [`dotnet-zoom-meetings`](src/ZoomMeetings.Cli) | Interactive CLI tool (`zoom-meetings ...`) to set up, inspect, verify, and run all 186 tools | `net8.0` |

### Installation

```bash
# Core client library
dotnet add package ZoomMeetings

# ASP.NET Core DI & Webhooks (optional)
dotnet add package ZoomMeetings.AspNetCore

# Global CLI tool (optional)
dotnet tool install -g dotnet-zoom-meetings
```

---

## Quick Setup: Auto-Detect Platform & Activate All 186 Tools

The CLI tool auto-detects your host project platform and shell environment, generating the optimal configuration format and starter integration code accordingly:

```bash
# Run the setup wizard in your project root
zoom-meetings setup
```

The CLI inspects your environment and automatically:
1. **Detects your project type**:
   - **ASP.NET Core (Minimal API / MVC / Blazor)**: Generates and formats `appsettings.Development.json` (or `appsettings.json`) under the `"Zoom"` section.
   - **Azure Functions (.NET Isolated Worker)**: Generates and formats `local.settings.json` under `"Values"` (`Zoom:AccountId`, `Zoom__AccountId`).
   - **Docker / Polyglot / Generic**: Generates standard `.env` variables (`ZOOM_ACCOUNT_ID`, etc.).
2. **Detects your shell & OS**:
   - Windows PowerShell: Generates `zoom-env.ps1` (`$env:ZOOM_*`).
   - Linux / macOS (Bash / Zsh): Generates `zoom-env.sh` (`export ZOOM_*`).
3. **Scaffolds starter code**:
   - Pass `--generate-code` or run `zoom-meetings generate` to produce platform-tailored boilerplate (`ZoomEndpoints.cs`, `ZoomMeetingFunctions.cs`, `ZoomDashboard.razor`, or `ZoomQuickstart.cs`).
4. **Verifies live connectivity**:
   - Immediately tests OAuth authentication against Zoom and confirms activation of all 186 tools across all 13 domains.

You can also run setup non-interactively:

```bash
zoom-meetings setup \
  --account-id "your_account_id" \
  --client-id "your_client_id" \
  --client-secret "your_client_secret" \
  --generate-code
```

### Getting Zoom Credentials (Server-to-Server OAuth)

Zoom's Meetings API uses **Server-to-Server OAuth** apps:
1. Go to the [Zoom App Marketplace](https://marketplace.zoom.us/) → **Develop** → **Build App** → **Server-to-Server OAuth**.
2. Under **Scopes**, select the permissions you need (e.g. `meeting:read:admin`, `meeting:write:admin`, `recording:read:admin`, `webinar:write:admin`, etc.).
3. Copy the **Account ID**, **Client ID**, and **Client Secret** from the **App Credentials** tab.
4. Click **Activate** to activate the app.

---

## Points of Interest: How This Extension Serves You

When you install this library or CLI, you get a complete toolchain across 13 functional domains. Here is how each toolset serves your applications:

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                      ZoomMeetings Tool Suite (186 Operations)                   │
├───────────────────────┬─────────────────────────┬───────────────────────────────┤
│ Core Automation       │ Content & Media         │ Enterprise & Administration   │
│ • Meetings (27)       │ • Cloud Recordings (23) │ • Reports & Analytics (24)    │
│ • Registrants (8)     │ • AI Summaries (4)      │ • Zoom Rooms & Devices (17)   │
│ • Webinars (53)       │ • Polls & Quizzes (7)   │ • SIP Phones & Telephony (12) │
│ • Templates (2)       │ • Live Controls (4)     │ • Tracking Fields (5)         │
└───────────────────────┴─────────────────────────┴───────────────────────────────┘
```

### 1. Automated Meeting Lifecycle (27 tools)
Automate the entire meeting lifecycle without leaving C#:
- **Scheduling**: Create instant, scheduled, or recurring meetings with passcodes, waiting rooms, and customized host settings.
- **Invitations & Links**: Retrieve formatted invitation text or generate customized attendee invite links.
- **Join Tokens**: Generate streaming join tokens, local archiving tokens, and local recording tokens for custom client integrations.
- **Live Streaming**: Dispatch meetings to YouTube, Facebook, or custom RTMP endpoints with live broadcast controls.
- **Surveys & Post-Meeting**: Query past meeting instances, participants, Q&A transcripts, and post-session surveys.

```csharp
using ZoomMeetings;
using ZoomMeetings.Models;

using var client = new ZoomClient(config);

// Schedule a meeting with password and waiting room enabled
var meeting = await client.CreateMeetingAsync("me", new CreateMeetingRequest
{
    Topic = "Executive Strategy Review",
    Type = 2, // Scheduled meeting
    StartTime = DateTimeOffset.UtcNow.AddDays(1),
    Duration = 45,
    Settings = new MeetingSettings
    {
        HostVideo = true,
        ParticipantVideo = false,
        WaitingRoom = true,
        AutoRecording = "cloud"
    }
});

Console.WriteLine($"Meeting Created! Join URL: {meeting?.JoinUrl}");

// Get formatted invitation text ready to send via email
var invitation = await client.GetMeetingInvitationAsync(meeting!.Id.ToString());
Console.WriteLine(invitation?.Invitation);
```

### 2. AI Companion & Meeting Summaries (4 tools)
Unlock Zoom AI Companion data directly in your applications:
- **Executive Summaries**: Retrieve AI-generated meeting summaries, discussion highlights, and key takeaways.
- **Action Items**: Extract action item lists and chapters to sync into Jira, Linear, or CRM records.
- **Account-Wide & Per-User**: Query summaries across the account or filter by specific user.

```csharp
// Retrieve AI Companion summary and chapters for an ended meeting
var summary = await client.GetMeetingSummaryAsync(meetingId);

Console.WriteLine($"Summary: {summary?.SummaryTitle}");
Console.WriteLine(summary?.SummaryContent);

foreach (var item in summary?.NextSteps ?? Enumerable.Empty<string>())
{
    Console.WriteLine($"• Action item: {item}");
}
```

### 3. Cloud Recordings & AI Transcripts (23 tools)
Build complete media ingestion and archiving pipelines:
- **Media Assets**: Retrieve MP4 video, M4A audio, chat logs, and secure download tokens.
- **AI Audio Transcripts**: Fetch full speech-to-text transcripts in standard VTT format.
- **Viewer Registration**: Require attendee registration for on-demand recordings and approve/deny viewers.
- **Analytics**: Track viewer analytics (play count, download count, viewer engagement minutes).
- **Trash & Compliance Recovery**: Safely soft-delete, restore, or permanently purge recordings.

```csharp
// Retrieve recordings for a meeting
var recordings = await client.GetRecordingsAsync(meetingId);
foreach (var file in recordings?.RecordingFiles ?? Enumerable.Empty<RecordingFile>())
{
    Console.WriteLine($"{file.FileType} ({file.FileSize} bytes): {file.DownloadUrl}");
}

// Download the AI speech-to-text transcript (VTT format)
var transcript = await client.GetMeetingTranscriptAsync(meetingId);
Console.WriteLine($"Transcript ({transcript?.Length} bytes) ready for vector embedding or RAG!");
```

### 4. Webinars & Event Production (53 tools)
Produce large-scale interactive events for up to 10,000+ attendees:
- **Panelist Management**: Add, query, or delete panelists with dedicated join URLs.
- **Custom Branding**: Configure customized name tags, set virtual backgrounds, and upload custom wallpapers.
- **File Uploads**: Built-in multipart file upload support (`UploadFileAsync`) for virtual background and wallpaper graphics.
- **Tracking Sources**: Create trackable campaign registration links to monitor marketing ROI.

```csharp
// Schedule a high-capacity webinar
var webinar = await client.CreateWebinarAsync("me", new CreateWebinarRequest
{
    Topic = "Global Product Launch Keynote",
    StartTime = DateTimeOffset.UtcNow.AddDays(7),
    Duration = 60
});

// Add key speakers as panelists
await client.AddWebinarPanelistsAsync(webinar!.Id.ToString(), new AddWebinarPanelistsRequest
{
    Panelists = new List<WebinarPanelistInput>
    {
        new() { Name = "Jane Doe", Email = "jane.doe@example.com" },
        new() { Name = "John Smith", Email = "john.smith@example.com" }
    }
});

// Upload a custom branded wallpaper background
await using var wallpaperStream = File.OpenRead("banner.jpg");
await client.UploadWebinarBrandingWallpaperAsync(webinar.Id.ToString(), wallpaperStream, "banner.jpg");
```

### 5. Interactive Polls & Quizzes (7 tools)
Drive engagement and extract attendee sentiment:
- **Bulk Creation**: Create batch polls and quizzes with single-choice, multiple-choice, or open-ended questions.
- **Quiz Scoring**: Set correct answers and point values for training assessments.
- **Past Results**: Export poll voting results and attendee answers after the meeting.

```csharp
// Create a batch poll for an upcoming session
await client.CreateBatchPollsAsync(meetingId, new CreateBatchPollsRequest
{
    Polls = new List<CreatePollRequest>
    {
        new()
        {
            Title = "Product Feedback",
            PollType = 1,
            Questions = new List<PollQuestion>
            {
                new()
                {
                    Name = "How would you rate today's session?",
                    Type = "single",
                    Answers = new List<string> { "Excellent", "Good", "Needs Improvement" }
                }
            }
        }
    }
});
```

### 6. Registrant Workflows & Batch Import (8 tools)
Automate attendee onboarding:
- **Batch Registrants**: Bulk register up to 30 attendees in a single request with auto-approval.
- **Custom Questions**: Configure custom registration fields (e.g. Job Title, Company, Purchasing Intent).
- **Approval Lifecycle**: Automatically approve, cancel, or deny registrants.

```csharp
// Bulk register attendees in a single API call
await client.CreateBatchRegistrantsAsync(meetingId, new BatchRegistrantsRequest
{
    Registrants = new List<BatchRegistrantItem>
    {
        new() { Email = "alice@partner.com", FirstName = "Alice", LastName = "Wong" },
        new() { Email = "bob@client.com", FirstName = "Bob", LastName = "Miller" }
    }
});
```

### 7. Regulatory Archiving & Compliance (Recordings/Archiving)
Meet strict enterprise compliance mandates (FINRA, SEC, HIPAA):
- **Compliance Archives**: Query archived compliance media across the organization.
- **Download Audit Trail**: Inspect comprehensive audit logs of who downloaded compliance files, when, and from which IP.
- **Retention Controls**: Update retention durations and toggle per-file auto-delete settings.

```csharp
// Inspect archive file download audit logs
var audit = await client.ListArchiveFileDownloadAuditAsync(
    from: DateTime.UtcNow.AddDays(-30).ToString("yyyy-MM-dd"),
    to: DateTime.UtcNow.ToString("yyyy-MM-dd")
);
```

### 8. Deep Analytics & 24 Reporting Engines (24 tools)
Full visibility into account usage, attendance, and operations:
- **Attendance Metrics**: Retrieve participant join time, leave time, and duration down to the second.
- **System Audit Logs**: Audit administrator operation logs, sign-in/sign-out activities, and disclaimer acknowledgments.
- **Billing & Telephony**: Track cloud recording storage, telephone dial-in toll minutes, and active licenses.

```csharp
// Get exact participant attendance duration report
var report = await client.GetMeetingParticipantsReportAsync(meetingId);
foreach (var participant in report?.Participants ?? Enumerable.Empty<ReportParticipant>())
{
    Console.WriteLine($"{participant.Name} ({participant.UserEmail}): attended {participant.Duration} minutes");
}
```

### 9. Live In-Meeting Controls & Moderation (4 tools)
Moderate active sessions in real time:
- **Chat Moderation**: Edit or permanently delete inappropriate chat messages sent during a live session.
- **In-Meeting Actions**: Remove participants, trigger room system callouts, update waiting room title/description, or toggle AI Companion mode.
- **RTMS App Streaming**: Update real-time media streaming (RTMS) status for participant video feeds.

```csharp
// Send in-meeting control event (e.g. remove a participant or update waiting room)
await client.InMeetingControlAsync(meetingId, new InMeetingControlRequest
{
    Method = "waiting_room.update_title_and_description",
    Params = new InMeetingControlParams
    {
        WaitingRoomTitle = "Session Starting Soon",
        WaitingRoomDescription = "Please wait, the host will admit you at 10:00 AM."
    }
});

// Delete a disruptive chat message from a live meeting
await client.DeleteLiveMeetingChatMessageAsync(meetingId, messageId);
```

### 10. Reusable Meeting Templates (2 tools)
Standardize corporate meeting policies:
- **Template Listing**: Enumerate organization meeting templates available for a user.
- **Create from Meeting**: Capture all settings from a successful meeting into a reusable template.

```csharp
// Convert a meeting into a company-wide reusable template
await client.CreateMeetingTemplateAsync("me", new CreateMeetingTemplateRequest
{
    MeetingId = meetingId,
    Name = "All-Hands Template",
    SaveRecurrence = true
});
```

### 11. Hardware, Zoom Rooms & H.323/SIP Systems (17 tools)
Bridge modern cloud meetings with corporate hardware:
- **Zoom Rooms & ZDM**: Query enrolled hardware devices, device groups, OS versions, and tags.
- **Zoom Phone Appliances (ZPA)**: Assign ZPA devices to common areas, trigger remote firmware upgrades, and manage profile settings.
- **H.323/SIP Endpoints**: Register conference room systems with IP addresses and auto-encryption protocols.

```csharp
// List enrolled Zoom Room devices
var devices = await client.ListDevicesAsync();

// Register a Poly / Cisco H.323 conference room endpoint
var h323 = await client.CreateH323DeviceAsync(new CreateH323DeviceRequest
{
    Name = "Executive Boardroom",
    Ip = "192.168.1.100",
    Protocol = "H.323",
    Encryption = "auto"
});
```

### 12. SIP Telephony & TSP Audio Conferencing (12 tools)
Comprehensive telephony integration:
- **SIP Phones**: Provision, list, and configure SIP phone devices with transport protocols.
- **TSP Integration**: Manage 3rd party Telephony Service Provider credentials, audio conferencing settings, and global dial-in URLs.

```csharp
// Inspect account Telephony Service Provider settings
var tsp = await client.GetAccountTspAsync();
```

### 13. Tracking Fields & Cost Centers (5 tools)
Enforce billing attribution:
- Create custom tracking fields (e.g., Cost Center, Client ID, Project Code).
- Mark tracking fields as mandatory or optional across all meeting schedules.

```csharp
// Create a mandatory Cost Center tracking field
await client.CreateTrackingFieldAsync(new CreateTrackingFieldRequest
{
    Field = "Cost Center",
    Required = true,
    Visible = true
});
```

---

## Webhooks & Security Handshake (ASP.NET Core)

Zoom requires webhook endpoints to satisfy an automated `endpoint.url_validation` CRC challenge and verify the `X-Zm-Signature` HMAC-SHA256 header. `ZoomMeetings.AspNetCore` handles both automatically:

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddZoomMeetings(builder.Configuration);

var app = builder.Build();

// Automatically validates CRC challenge and verifies HMAC-SHA256 signature
app.MapZoomWebhook("/webhooks/zoom", secretToken: builder.Configuration["Zoom:WebhookSecretToken"]!, async (payload, context) =>
{
    var eventType = payload.GetProperty("event").GetString();
    
    switch (eventType)
    {
        case "meeting.started":
            // Handle meeting start
            break;
        case "recording.completed":
            // Trigger automatic download pipeline
            break;
    }
});

app.Run();
```

---

## Complete CLI Tool Reference

The `zoom-meetings` CLI tool lets you interact with, script, and test all 186 tools directly from your terminal or CI/CD pipelines.

### Setup & Discovery
```bash
# Setup wizard (auto-detects platform, config target, and shell)
zoom-meetings setup

# Auto-detect platform and generate starter integration code & environment scripts
zoom-meetings generate
zoom-meetings generate code        # Generates ZoomEndpoints.cs, ZoomFunctions.cs, or ZoomDashboard.razor
zoom-meetings generate env         # Generates zoom-env.ps1 (PowerShell) or zoom-env.sh (Bash)
zoom-meetings generate --platform minimal-api  # Force a specific platform template

# Explore all 13 tool domains
zoom-meetings tools

# List all 186 tools with HTTP methods and paths
zoom-meetings tools all

# Filter tools by domain
zoom-meetings tools meetings
zoom-meetings tools recordings
zoom-meetings tools webinars
zoom-meetings tools live
zoom-meetings tools devices

# Search tools by keyword across names, paths, and descriptions
zoom-meetings tools search transcript
zoom-meetings tools search branding
zoom-meetings tools search poll

# Verify credentials and tool readiness against live Zoom account
zoom-meetings tools verify

# Export tool catalog as machine-readable JSON (ideal for agent tooling / MCP)
zoom-meetings tools json

# Confirm active profile identity
zoom-meetings whoami
```

### Common CLI Operations
```bash
# Meetings
zoom-meetings meetings list --user-id me
zoom-meetings meetings create --user-id me --topic "Quarterly Sync" --start-time 2026-10-01T09:00:00Z
zoom-meetings meetings get --meeting-id 123456789

# Registrants
zoom-meetings registrants list --meeting-id 123456789
zoom-meetings registrants add --meeting-id 123456789 --email user@example.com --first-name Jane --last-name Doe

# Polls
zoom-meetings polls list --meeting-id 123456789

# Cloud Recordings & Summaries
zoom-meetings recordings get --meeting-id 123456789
zoom-meetings summaries get --meeting-id 123456789

# Webinars
zoom-meetings webinars list --user-id me

# Hardware & Devices
zoom-meetings devices list
zoom-meetings devices h323-list

# Raw Escape Hatch (execute any raw Zoom endpoint)
zoom-meetings raw GET /users/me
```

> **Git Bash on Windows**: MSYS2 automatically converts arguments starting with `/` (such as `raw GET /users/me`). Prefix commands with `MSYS_NO_PATHCONV=1` or run from PowerShell / cmd.

---

## 186 Tools Coverage Summary

All 186 operations defined in Zoom's OpenAPI specification are 100% typed. See [docs/ENDPOINT-COVERAGE.md](docs/ENDPOINT-COVERAGE.md) for the complete, spec-audited mapping.

| Domain | Total Operations | Typed in v1 | Key Methods |
|---|---|---|---|
| **Meetings core** | 27 | **27 (100%)** | `CreateMeetingAsync`, `GetMeetingAsync`, `UpdateMeetingAsync`, `DeleteMeetingAsync`, `GetMeetingInvitationAsync`, `GetMeetingTokenAsync`, `GetPastMeetingParticipantsAsync`, `GetPastMeetingQaAsync`, ... |
| **Registrants** | 8 | **8 (100%)** | `ListRegistrantsAsync`, `AddRegistrantAsync`, `CreateBatchRegistrantsAsync`, `UpdateRegistrantStatusAsync`, `GetRegistrationQuestionsAsync`, ... |
| **Polls & Quizzes** | 7 | **7 (100%)** | `CreateBatchPollsAsync`, `ListPollsAsync`, `CreatePollAsync`, `GetPollAsync`, `UpdatePollAsync`, `GetPastMeetingPollsAsync`, ... |
| **Recordings & Archiving** | 23 | **23 (100%)** | `GetRecordingsAsync`, `DeleteRecordingsAsync`, `GetMeetingTranscriptAsync`, `GetRecordingAnalyticsDetailsAsync`, `ListArchivedFilesAsync`, `RecoverRecordingFileAsync`, ... |
| **AI Meeting Summaries** | 4 | **4 (100%)** | `GetMeetingSummaryAsync`, `ListMeetingSummariesAsync`, `ListUserMeetingSummariesAsync`, `DeleteMeetingSummaryAsync` |
| **Reports & Analytics** | 24 | **24 (100%)** | `GetActivitiesReportAsync`, `GetMeetingParticipantsReportAsync`, `GetDailyReportAsync`, `GetCloudRecordingReportAsync`, `GetBillingReportAsync`, `GetOperationLogsReportAsync`, ... |
| **Webinars** | 53 | **53 (100%)** | `CreateWebinarAsync`, `ListWebinarPanelistsAsync`, `AddWebinarPanelistsAsync`, `UploadWebinarBrandingWallpaperAsync`, `UploadWebinarBrandingVirtualBackgroundAsync`, `CreateBatchWebinarRegistrantsAsync`, ... |
| **Meeting Templates** | 2 | **2 (100%)** | `ListMeetingTemplatesAsync`, `CreateMeetingTemplateAsync` |
| **Live Meeting Controls** | 4 | **4 (100%)** | `DeleteLiveMeetingChatMessageAsync`, `UpdateLiveMeetingChatMessageAsync`, `InMeetingControlAsync`, `UpdateMeetingRtmsStatusAsync` |
| **Tracking Fields** | 5 | **5 (100%)** | `ListTrackingFieldsAsync`, `CreateTrackingFieldAsync`, `GetTrackingFieldAsync`, `UpdateTrackingFieldAsync`, `DeleteTrackingFieldAsync` |
| **SIP Phones** | 4 | **4 (100%)** | `ListSipPhonesAsync`, `EnableSipPhoneAsync`, `UpdateSipPhoneAsync`, `DeleteSipPhoneAsync` |
| **Telephony (TSP)** | 8 | **8 (100%)** | `GetAccountTspAsync`, `UpdateAccountTspAsync`, `ListUserTspsAsync`, `CreateUserTspAsync`, `UpdateUserTspUrlAsync`, `GetUserTspAsync`, ... |
| **Devices & H.323** | 17 | **17 (100%)** | `ListDevicesAsync`, `CreateDeviceAsync`, `ListDeviceGroupsAsync`, `AssignZpaDeviceAsync`, `UpgradeZpaAsync`, `ListH323DevicesAsync`, `CreateH323DeviceAsync`, ... |
| **Total** | **186** | **186 (100%)** | **Complete coverage across every endpoint** |

---

## The Raw Escape Hatch

Every typed method is built upon `ZoomClient.CallAsync`. If Zoom introduces a brand-new beta endpoint tomorrow, you never have to wait for a library update:

```csharp
// Call any current or future Zoom API endpoint immediately
var result = await client.CallAsync<JsonElement>(HttpMethod.Get, "/users/me");
```

---

## Pagination Made Simple

List endpoints in the Zoom API use `next_page_token`. You never need to write token-paging loops yourself — simply use `IAsyncEnumerable<T>` wrappers:

```csharp
// Automatically walks all pages sequentially
await foreach (var meeting in client.EnumerateMeetingsAsync("me"))
{
    Console.WriteLine($"Meeting: {meeting.Topic} ({meeting.Id})");
}
```

---

## Multiple Profiles & Sandbox Testing

Because Zoom does not offer a separate sandbox URL, staging is typically done with a second Server-to-Server app under a test account. `ZoomMeetings` supports named environments with keyed services:

```csharp
// Register multiple accounts in ASP.NET Core
builder.Services.AddZoomMeetings(builder.Configuration, sectionName: "Zoom:Production", name: "Production");
builder.Services.AddZoomMeetings(builder.Configuration, sectionName: "Zoom:Sandbox", name: "Sandbox");
```

```csharp
// Inject by key
app.MapPost("/test-meeting", async ([FromKeyedServices("Sandbox")] ZoomClient sandboxClient) =>
{
    return await sandboxClient.CreateMeetingAsync("me", new CreateMeetingRequest { Topic = "Sandbox Test" });
});
```

Using the CLI with named profiles:
```bash
zoom-meetings setup --profile Sandbox --account-id ... --client-id ... --client-secret ...
zoom-meetings whoami --profile Sandbox
```

---

## Error Handling

Failed API requests throw `ZoomApiException`, giving you direct access to HTTP status codes and Zoom's error details:

```csharp
try
{
    await client.GetMeetingAsync("invalid_id");
}
catch (ZoomApiException ex)
{
    Console.WriteLine($"HTTP Status : {ex.StatusCode}");
    Console.WriteLine($"Zoom Code   : {ex.ZoomCode}");
    Console.WriteLine($"Message     : {ex.ZoomMessage}");
}
```

---

## Design Principles & Compatibility

- **Forward & Backward Compatibility**: Multi-targets `netstandard2.0;net8.0;net10.0`. Works on everything from modern .NET 10 to .NET Framework 4.6.1 and legacy runtimes.
- **Resilient Models**: Request models omit null fields on serialization (`WhenWritingNull`), preventing unintended overrides. Response models preserve unknown fields using `[JsonExtensionData]` so newly added Zoom fields are never lost.
- **Double-Encoding Safety**: Automatically handles Zoom's double-encoding rules for meeting UUIDs containing `/` or `//`.
- **Automatic Rate-Limit Backoff**: Transparently retries HTTP 429 responses, strictly honoring Zoom's `Retry-After` header.

---

## Examples

Check out [`examples/`](examples) for ready-to-run projects:
- `minimal-api/` — ASP.NET Core Minimal API with DI and webhook validation
- `mvc/` — ASP.NET Core MVC controllers
- `blazor-server/` — Interactive Blazor Server dashboard
- `azure-functions/` — Serverless Azure Functions (.NET isolated worker)
- `all-186-endpoints/` — Live verification suite executing all 186 endpoints

---

## License

[MIT](LICENSE)
