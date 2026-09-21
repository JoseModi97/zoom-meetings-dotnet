# Changelog

This project follows [Semantic Versioning](https://semver.org/). Until 1.0.0, minor versions (`0.x.0`)
may still contain breaking changes to the public API; from 1.0.0 onward, breaking changes require a
major version bump.

## [0.1.0] - Unreleased

Initial release.

- `ZoomClient` with typed methods for Meetings, Registrants, Polls, Cloud Recordings, Meeting
  Summaries, Webinars, and two Reports endpoints (~40 of Zoom's ~186 Meetings-API operations —
  see `docs/ENDPOINT-COVERAGE.md`), plus `CallAsync` reaching every other operation.
- Server-to-Server OAuth2 (account_credentials grant) with token caching/refresh and 401 retry.
- 429 retry with `Retry-After` handling.
- `ZoomMeetings.AspNetCore`: `AddZoomMeetings(...)` DI registration (including named/keyed
  registrations for multiple Zoom accounts, since Zoom has no separate sandbox API) and
  `MapZoomWebhook(...)` webhook signature verification.
- `dotnet-zoom-meetings` CLI.
