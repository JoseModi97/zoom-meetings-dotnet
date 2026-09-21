# ZoomMeetings - Multi-Platform Examples

This folder contains clean, self-contained examples demonstrating how to integrate the **`ZoomMeetings`** and **`ZoomMeetings.AspNetCore`** packages across various .NET platforms and architectural styles. They sit outside `ZoomMeetings.sln` on purpose — each is a standalone project you can copy elsewhere.

> **Security note**: None of the examples include real credentials. All examples use placeholder values (`YOUR_ACCOUNT_ID`, `YOUR_CLIENT_ID`, etc.) or read from environment variables / `appsettings.json`. They reference the local `src/` projects via `ProjectReference` since these packages aren't published to NuGet.org yet — swap to a `PackageReference` once they are (see the comment in each `.csproj`).

---

## Directory Index

| Platform / Template | Description | Key Features |
|---|---|---|
| [`aspnetcore-minimal-api/`](./aspnetcore-minimal-api) | ASP.NET Core Minimal API (.NET 8) | `AddZoomMeetings()`, list/create meeting endpoints, `MapZoomWebhook()` |
| [`aspnetcore-mvc/`](./aspnetcore-mvc) | ASP.NET Core MVC Controller (.NET 8) | `MeetingsController` (list/create/delete), constructor-injected `ZoomClient` |
| [`blazor-server/`](./blazor-server) | Blazor Web App, interactive server render mode (.NET 8) | A real `.razor` page listing meetings with a create form, injected `ZoomClient` |
| [`azure-functions-worker/`](./azure-functions-worker) | Azure Functions isolated worker (.NET 8) | HTTP-triggered functions for listing and creating meetings |
| [`console-script/`](./console-script) | Lightweight console application (.NET 8) | Pagination via `EnumerateMeetingsAsync`, typed create, and the raw `CallAsync` escape hatch |

---

## Quick configuration checklist

To run any example against your own Zoom account, supply credentials via either:

### 1. `appsettings.json` (recommended for web apps)

```json
{
  "Zoom": {
    "AccountId": "YOUR_ACCOUNT_ID",
    "ClientId": "YOUR_CLIENT_ID",
    "ClientSecret": "YOUR_CLIENT_SECRET",
    "WebhookSecretToken": "YOUR_WEBHOOK_SECRET_TOKEN"
  }
}
```

### 2. Environment variables (recommended for the console script, Azure Functions, and CI)

```bash
export ZOOM_ACCOUNT_ID="YOUR_ACCOUNT_ID"
export ZOOM_CLIENT_ID="YOUR_CLIENT_ID"
export ZOOM_CLIENT_SECRET="YOUR_CLIENT_SECRET"
```

See the root [README](../README.md) for how to create a Server-to-Server OAuth app in the Zoom Marketplace and obtain these values.
