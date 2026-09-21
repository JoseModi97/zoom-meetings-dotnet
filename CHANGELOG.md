# Changelog

This project follows [Semantic Versioning](https://semver.org/). Until 1.0.0, minor versions (`0.x.0`)
may still contain breaking changes to the public API; from 1.0.0 onward, breaking changes require a
major version bump.

## [0.1.0] - Unreleased

Initial release.

- `ZoomClient` with typed methods for 140 of Zoom's 186 Meetings-API operations — full coverage of
  Meetings core, Recordings/Archiving, Reports, and Webinars, plus Registrants, Polls, and Meeting
  Summaries (see `docs/ENDPOINT-COVERAGE.md`). The remaining 46 operations (Devices/H.323, SIP
  Phones, TSP, Tracking Fields, Templates, Live Meeting Controls, and a few batch/question
  endpoints) are reachable via `CallAsync`/`UploadFileAsync`, which reach every operation in the
  spec regardless of typed-method coverage.
- Server-to-Server OAuth2 (account_credentials grant) with token caching/refresh and 401 retry.
- 429 retry with `Retry-After` handling.
- `ZoomMeetings.AspNetCore`: `AddZoomMeetings(...)` DI registration (including named/keyed
  registrations for multiple Zoom accounts, since Zoom has no separate sandbox API) and
  `MapZoomWebhook(...)` webhook signature verification.
- `dotnet-zoom-meetings` CLI.
