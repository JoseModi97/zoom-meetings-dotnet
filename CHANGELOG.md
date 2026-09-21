# Changelog

This project follows [Semantic Versioning](https://semver.org/). Until 1.0.0, minor versions (`0.x.0`)
may still contain breaking changes to the public API; from 1.0.0 onward, breaking changes require a
major version bump.

## [0.2.0] - 2026-09-21

- Complete 100% typed coverage across all 186 Zoom Meetings REST API endpoints and 13 domains.
- Added full 186 tools catalog discovery in CLI (`zoom-meetings tools [domain|search|verify|json]`).
- Added smart environment & platform auto-detection and code generator (`zoom-meetings setup`, `zoom-meetings generate`).
- Added full sample integrations for Minimal API, MVC, Blazor Server, Azure Functions, and an all-186 verification suite.
- Added Pesapal open-source sponsorship support and repository metadata.

## [0.1.0] - 2026-09-21

Initial release.

- `ZoomClient` with typed methods for all 186 of Zoom's Meetings-API operations (100% coverage
  across all 13 domains: Meetings core, Recordings/Archiving, Reports, Webinars, Registrants, Polls,
  Meeting Summaries, Devices/H.323, SIP Phones, TSP, Tracking Fields, Templates, and Live Meeting
  Controls — see `docs/ENDPOINT-COVERAGE.md`).
- `CallAsync` and `UploadFileAsync` escape hatches available for raw requests and custom payloads.
- Server-to-Server OAuth2 (account_credentials grant) with token caching/refresh and 401 retry.
- 429 retry with `Retry-After` handling.
- `ZoomMeetings.AspNetCore`: `AddZoomMeetings(...)` DI registration (including named/keyed
  registrations for multiple Zoom accounts, since Zoom has no separate sandbox API) and
  `MapZoomWebhook(...)` webhook signature verification.
- `dotnet-zoom-meetings` CLI.
