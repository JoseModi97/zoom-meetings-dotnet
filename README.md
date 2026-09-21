# ZoomMeetings — Zoom Meetings API Client for .NET

Idiomatic .NET / C# client for the [Zoom Meetings REST API](https://developers.zoom.us/docs/api/): meetings, registrants, polls, cloud recordings, AI meeting summaries, webinars, and reports, with Server-to-Server OAuth2 built in. Every one of Zoom's ~186 Meetings-API operations is reachable on day one — the most common ones through typed, ergonomic C# methods, everything else through a generic escape hatch.

> **Unofficial.** This project is not affiliated with, endorsed by, or sponsored by Zoom Video Communications, Inc. "Zoom" is a trademark of Zoom Video Communications, Inc.

## Packages

| Package | Description | Target frameworks |
|---|---|---|
| [`ZoomMeetings`](src/ZoomMeetings) | Core client (`ZoomClient`) — no ASP.NET Core dependency | `netstandard2.0`, `net8.0`, `net10.0` |
| [`ZoomMeetings.AspNetCore`](src/ZoomMeetings.AspNetCore) | `AddZoomMeetings(...)` DI registration + webhook signature verification | `net8.0`, `net10.0` |
| [`dotnet-zoom-meetings`](src/ZoomMeetings.Cli) | Global CLI tool (`zoom-meetings ...`) | `net8.0` |

## Installation

```bash
dotnet add package ZoomMeetings
dotnet add package ZoomMeetings.AspNetCore   # optional, for ASP.NET Core apps
dotnet tool install -g dotnet-zoom-meetings  # optional, CLI
```

## Getting Zoom credentials (Server-to-Server OAuth)

Zoom's Meetings API is authenticated with a **Server-to-Server OAuth app**, not a personal API key:

1. Go to the [Zoom App Marketplace](https://marketplace.zoom.us/) → **Develop** → **Build App** → **Server-to-Server OAuth**.
2. Add the scopes you need (e.g. `meeting:write:meeting`, `meeting:read:meeting`, `meeting:read:list_meetings`, `webinar:write:webinar`, ...) under **Scopes**.
3. Copy the **Account ID**, **Client ID**, and **Client Secret** from the app's **App Credentials** page.
4. Activate the app.

Zoom does **not** offer a separate sandbox API host (confirmed against Zoom's own developer forum — this is a long-standing, frequently-requested gap). To test safely, create a **second** Server-to-Server app under a dedicated test Zoom account and register it under a different name/profile (see [Sandbox / multiple environments](#sandbox--multiple-environments) below) rather than pointing at a different URL.

## Quickstart (plain console app)

```csharp
using ZoomMeetings;
using ZoomMeetings.Models;

var config = new ZoomConfig
{
    AccountId = "...",     // or set ZOOM_ACCOUNT_ID
    ClientId = "...",      // or set ZOOM_CLIENT_ID
    ClientSecret = "...",  // or set ZOOM_CLIENT_SECRET
};

using var client = new ZoomClient(config);

var meeting = await client.CreateMeetingAsync("me", new CreateMeetingRequest
{
    Topic = "Sprint planning",
    StartTime = DateTimeOffset.UtcNow.AddDays(1),
    Duration = 30,
});

Console.WriteLine(meeting?.JoinUrl);
```

## Quickstart (ASP.NET Core / dependency injection)

```csharp
// Program.cs
builder.Services.AddZoomMeetings(builder.Configuration); // binds the "Zoom" appsettings section
```

```json
// appsettings.json
{
  "Zoom": {
    "AccountId": "...",
    "ClientId": "...",
    "ClientSecret": "..."
  }
}
```

```csharp
app.MapGet("/meetings/{id}", async (string id, ZoomClient zoom) => await zoom.GetMeetingAsync(id));
```

Or configure with a delegate instead of `IConfiguration`:

```csharp
builder.Services.AddZoomMeetings(config =>
{
    config.AccountId = builder.Configuration["Zoom:AccountId"]!;
    config.ClientId = builder.Configuration["Zoom:ClientId"]!;
    config.ClientSecret = builder.Configuration["Zoom:ClientSecret"]!;
});
```

## Typed operations

**140 of Zoom's 186 Meetings-API operations are typed** — every operation in Meetings core, Recordings/Archiving, Reports, and Webinars (100% each). The remaining 46 are lower-traffic admin/device management endpoints (Devices/H.323, SIP Phones, TSP, Tracking Fields, Templates, Live Meeting Controls) plus a handful of batch/question endpoints under Registrants and Polls — **not unsupported, just not hand-typed yet**, and fully reachable via the raw escape hatch below. See [docs/ENDPOINT-COVERAGE.md](docs/ENDPOINT-COVERAGE.md) for the full, spec-generated per-operation list.

| Resource | Methods |
|---|---|
| Meetings | Create/Get/Update/Delete/List/Enumerate/ListUpcoming/UpdateStatus/GetInvitation, plus invite links, join tokens (live streaming/local archiving/local recording), livestream get/update/status, open apps add/delete, SIP dialing, survey get/update/delete, token, past-meeting details/instances/participants/Q&A, and a user's PAC accounts |
| Registrants | List/Enumerate/Add/Get/Delete/UpdateStatus (shared by meetings and webinars) |
| Polls | List/Create/Get/Update/Delete (shared by meetings and webinars) |
| Cloud Recordings | Get/Delete/settings get-update, recover (single file and whole meeting), analytics details/summary, recording registrants (list/add/questions get-update/status), transcript get/delete, a user's full recordings list |
| Archiving | Account-wide archived-files list/statistics/download-audit, per-file auto-delete update, per-meeting archived files get/delete |
| Meeting Summaries | Get/List(account)/Delete |
| Reports | All 22 `/report/*` endpoints: activities, billing (+invoices), cloud recording usage, daily usage, disclaimer, history, meeting activity logs, meeting/webinar detail+participants+polls+Q&A+survey, operation logs, remote support, telephone, upcoming events, user/host reports |
| Webinars | Create/Get/Update/Delete/List/Enumerate, registrants (incl. registration questions, batch add), invite links, join tokens, livestream, SIP dialing, survey, token, status, tracking sources, templates, past-webinar absentees/instances/participants/polls/Q&A, polls, panelists, and full branding (name tags, virtual backgrounds, wallpaper — including the two file-upload endpoints via `UploadFileAsync`) |

## The raw escape hatch

Every typed method above is a thin wrapper over `ZoomClient.CallAsync`, which is public and reaches **any** Zoom Meetings API operation immediately — pass the path exactly as [Zoom's API reference](https://developers.zoom.us/docs/api/) shows it:

```csharp
// e.g. an operation with no typed method yet: POST /meetings/{meetingId}/batch_polls
var result = await client.CallAsync<JsonElement>(
    HttpMethod.Post,
    $"/meetings/{meetingId}/batch_polls",
    body: new { polls = new[] { new { title = "Q1", questions = new[] { /* ... */ } } } });
```

## Pagination

Zoom's list endpoints use `next_page_token`. `ZoomPaging.EnumerateAsync` (and the `Enumerate*Async` convenience wrappers like `EnumerateMeetingsAsync`) walk every page for you as an `IAsyncEnumerable<T>`:

```csharp
await foreach (var meeting in client.EnumerateMeetingsAsync("me"))
{
    Console.WriteLine(meeting.Topic);
}
```

## Sandbox / multiple environments

Since Zoom has no real sandbox API, register a second app under a name:

```csharp
builder.Services.AddZoomMeetings(builder.Configuration, sectionName: "Zoom:Production", name: "Production");
builder.Services.AddZoomMeetings(builder.Configuration, sectionName: "Zoom:Sandbox", name: "Sandbox");
```

```csharp
app.MapPost("/test", ([FromKeyedServices("Sandbox")] ZoomClient sandboxClient) => ...);
```

## Webhooks

`ZoomMeetings.AspNetCore` verifies Zoom's `X-Zm-Signature` header and answers the required `endpoint.url_validation` handshake automatically:

```csharp
app.MapZoomWebhook("/webhooks/zoom", secretToken: builder.Configuration["Zoom:WebhookSecretToken"]!, async (payload, context) =>
{
    var eventType = payload.GetProperty("event").GetString();
    // handle meeting.started, meeting.participant_joined, etc.
});
```

The webhook secret token is a separate credential from your Server-to-Server app's client ID/secret — find it on the same app's **Feature → Event Subscriptions** page.

## CLI

```bash
zoom-meetings whoami
zoom-meetings meetings list --user-id me
zoom-meetings meetings create --user-id me --topic "Sprint planning" --start-time 2026-10-01T09:00:00Z
zoom-meetings registrants list --meeting-id 123456789
zoom-meetings raw GET /meetings/123456789/batch_polls --profile Sandbox
zoom-meetings --help
```

Credentials come from `ZOOM_ACCOUNT_ID`/`ZOOM_CLIENT_ID`/`ZOOM_CLIENT_SECRET` (or `ZOOM_<PROFILE>_*` when `--profile <name>` is passed), or an `appsettings.json` under a matching `Zoom`/`Zoom:<Profile>` section walked up from the current directory.

> **Git Bash on Windows**: MSYS2's automatic path conversion rewrites any argument starting with `/` (like `raw GET /meetings/123`) before the CLI ever sees it, which breaks `raw` and any path-taking command. Prefix the command with `MSYS_NO_PATHCONV=1` (or run from PowerShell/cmd instead), e.g. `MSYS_NO_PATHCONV=1 zoom-meetings raw GET /users/me/meetings`.

## More examples

[`examples/`](examples) has standalone, buildable projects for ASP.NET Core Minimal API, MVC, Blazor Server (an actual interactive `.razor` page, not just an endpoint), Azure Functions (isolated worker), and a console script — see [examples/README.md](examples/README.md).

## Error handling

Non-2xx responses throw `ZoomApiException` with `StatusCode`, Zoom's own `ZoomCode`/`ZoomMessage` (when the response body could be parsed — Zoom's error shape isn't part of the public OpenAPI spec, so this is best-effort), and the raw response body as a fallback.

## Versioning & compatibility

This project follows [Semantic Versioning](https://semver.org/) (see [CHANGELOG.md](CHANGELOG.md)). Until `1.0.0`, minor versions may still contain breaking changes; from `1.0.0` onward, a breaking change requires a major version bump.

**Framework compatibility (backward and forward):**

- `ZoomMeetings` multi-targets `netstandard2.0;net8.0;net10.0`. `netstandard2.0` covers everything back to .NET Framework 4.6.1, old .NET Core 2.x/3.x, Xamarin, Unity, etc. — consumers on older runtimes aren't left behind. `net8.0`/`net10.0` are the current and newest LTS releases. Because `netstandard2.0` is also a valid fallback target for *any future* .NET version, this package keeps working on .NET releases after 10.0 without needing a re-publish, even before this library adds an explicit TFM for them.
- `ZoomMeetings.AspNetCore` targets `net8.0;net10.0` (keyed DI services, used for the multi-environment/sandbox pattern, are a .NET 8+ feature).

**API compatibility with Zoom itself (forward compatibility with Zoom's own changes):**

- Response models keep the commonly-used fields typed and carry a `[JsonExtensionData]` bag for everything else — if Zoom adds a new field to a response, it's preserved (not dropped), and deserialization doesn't break.
- Status/type fields that Zoom documents as enumerated values (meeting `type`, registrant `status`, etc.) are typed as plain `int`/`string`, not C# `enum`s — if Zoom adds a new enum value in the future, it deserializes fine instead of throwing.
- Request models only serialize properties you actually set (`DefaultIgnoreCondition = WhenWritingNull`), so adding new optional properties to a request model in a future version of this library never starts sending unexpected fields to old Zoom API behavior.
- The `CallAsync` escape hatch means new Zoom operations are usable immediately via a raw path/body, without waiting for a new release of this library to add a typed method.

## Design notes

- **No code generation, no Refit.** `ZoomClient` is a plain concrete `partial class`, split by resource group — see [docs/API-ANALYSIS.md](docs/API-ANALYSIS.md) for the full analysis of Zoom's OpenAPI spec that this design is based on.
- **Meeting/webinar IDs are `string` everywhere**, not `long` — Zoom's own spec types the `meetingId` path parameter inconsistently (`integer` in 37 operations, `string` in 26, because recording/archive/report endpoints also accept the meeting UUID). `ZoomIdEncoding` double-encodes UUIDs containing `/` per Zoom's documented requirement.
- **429 responses are retried automatically**, honoring `Retry-After`.
- **Models keep the commonly-used fields typed** and carry a `[JsonExtensionData]` bag for the rest — Zoom's meeting `settings` object alone has 66-69 properties in the full spec.

## License

[MIT](LICENSE)
