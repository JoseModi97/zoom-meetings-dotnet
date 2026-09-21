# Zoom Meetings API — Analysis for .NET Client Extension

Source: `Meetings.json` (OpenAPI 3.0, ~1.2MB, Zoom's official public spec, v2)

## 1. Overview

- **API**: Zoom Meetings API (`https://api.zoom.us/v2`) — meetings, webinars, recordings/archiving, devices, reports, TSP, tracking fields, meeting summaries.
- **186 operations** across **131 paths**.
- **No `components.schemas`** — every request/response body is defined **inline**, per-operation. There is nothing to `$ref` against; a codegen tool will synthesize its own model names from operationId/path, which produces inconsistent/duplicate class names unless the tool is configured well (see §5).
- **43 of 186 operations (23%)** compose their schema with `allOf` (e.g. a generic "pagination wrapper" object combined with a "data" object). No `oneOf`/`anyOf` anywhere — so no polymorphic/discriminated unions to worry about.
- Bodies are richly documented: `description`, `example`, `maxLength`, `enum`, `default` are present almost everywhere — good raw material for XML doc comments on generated C# members.

## 2. Authentication

Two security schemes, used interchangeably per-operation (`security: [{...}]`), both really describing **Zoom Server-to-Server OAuth / OAuth2** in practice:

| Scheme | Type | Detail |
|---|---|---|
| `openapi_authorization` | `apiKey` in header `Authorization` | Raw bearer-token-in-header style (what you get from a Server-to-Server OAuth app: `Authorization: Bearer <token>`) |
| `openapi_oauth` | `oauth2`, `authorizationCode` flow | ~390 granular scopes (e.g. `meeting:write:meeting:admin`, `cloud_recording:read:list_recording_files`), each essentially 1:1 with an operation, often duplicated in `:admin` and `:master` variants |

**Implication for the extension**: don't try to model 390 individual OAuth scopes as C# constants tied to methods — that's Zoom's docs metadata, not something a caller supplies per-call. Model auth as a single pluggable `Authorization: Bearer {token}` header (via `DelegatingHandler`/`HttpMessageHandler`), and let the token come from either:
- a Server-to-Server OAuth credential flow (account ID + client id/secret → token endpoint `https://zoom.us/oauth/token`, not present in this spec — it lives on Zoom's OAuth spec, not the Meetings spec), or
- a caller-supplied `ITokenProvider`.

No API-key-in-query or Basic auth anywhere.

## 3. Endpoint inventory by domain

| Domain | Base path(s) | Ops | Notes |
|---|---|---|---|
| Meetings core | `/meetings/{meetingId}`, `/users/{userId}/meetings`, `/past_meetings/*` | ~20 | CRUD, status, invitation, invite links, SIP dialing, token, past-meeting detail/instances/participants/Q&A |
| Registrants (meeting) | `/meetings/{meetingId}/registrants*`, `/batch_registrants` | 7 | List/add/get/delete, batch, questions, status |
| Polls (meeting) | `/meetings/{meetingId}/polls*`, `/batch_polls`, `/past_meetings/{id}/polls` | 6 | CRUD + batch create + past results |
| Live meeting controls | `/live_meetings/{meetingId}/*` | 4 | Chat message update/delete, in-meeting controls, RTMS app status |
| Livestream | `/meetings/{meetingId}/livestream*` | 3 | Get/update details, update status |
| Join tokens | `/meetings/{meetingId}/jointoken/*` | 3 | local_archiving, local_recording, live_streaming |
| Survey | `/meetings/{meetingId}/survey` | 3 | Get/update/delete |
| Meeting summaries (AI) | `/meetings/meeting_summaries`, `/meetings/{meetingId}/meeting_summary`, `/users/{userId}/meeting_summaries` | 4 | List (account/user), get, delete |
| Recordings (cloud) | `/meetings/{meetingId}/recordings*`, `/users/{userId}/recordings` | ~14 | Get/delete, analytics, registrants+questions+status, settings, recover, transcript |
| Archiving | `/archive_files*`, `/past_meetings/{uuid}/archive_files` | 6 | List/statistics/download-audit, per-meeting archive files, auto-delete flag |
| Devices | `/devices*`, `/h323/devices*` | ~15 | Zoom Room/CRC device CRUD, groups, ZPA (Zoom Phone Appliance) assign/upgrade/settings, H.323/SIP device CRUD |
| SIP phones | `/sip_phones/phones*` | 3 | List/enable/update/delete |
| TSP | `/tsp*`, `/users/{userId}/tsp*` | 6 | Account + per-user telephony service provider config |
| Tracking fields | `/tracking_fields*` | 4 | CRUD for custom tracking field definitions |
| Templates | `/users/{userId}/meeting_templates`, `/users/{userId}/webinar_templates` | 2 | List/create |
| Reports | `/report/*` | ~22 | Billing, daily/activity usage, disclaimer, history, per-meeting/webinar detail+participants+polls+qna+survey, operation logs, remote support, telephone, upcoming events, users |
| Webinars | `/webinars/{webinarId}*`, `/users/{userId}/webinars*`, `/live_webinars/*`, `/past_webinars/*` | ~30 | Mirrors the meeting surface: CRUD, registrants, polls, panelists, branding (name tags/virtual backgrounds/wallpaper), livestream, survey, tracking sources, invite links, join tokens |

Full per-operation detail (operationId, params, request/response shapes) was extracted during this analysis — happy to regenerate that file if useful for the codegen step.

## 4. Data modeling characteristics (the hard part)

1. **Deeply nested, huge inline objects.** The "create/update meeting" body (`POST /users/{userId}/meetings`, `PATCH /meetings/{meetingId}`) has a top-level `settings` object with **66–69 properties** (many booleans/enums controlling waiting room, breakout rooms, auth, email notifications, etc.) plus an 8-property `recurrence` object. This single type will dominate the generated model surface.
2. **`allOf` composition pattern.** List endpoints typically compose a generic pagination object (`next_page_token`, `page_size`, `page_count`, `total_records`) with a domain-specific `{ items: [...] }` object via `allOf`. A codegen tool needs to *flatten* these into one C# class — plain `System.Text.Json` won't do this for free from two merged JSON Schema fragments; NSwag/Kiota handle it, hand-rolled `HttpClient` + manual DTOs would need to replicate the flattening by hand.
3. **Inconsistent `meetingId` parameter typing.** Across the 186 ops, the `meetingId` path parameter is typed `integer` in 37 operations and `string` in 26 (recording/archiving/report endpoints use `string` because they also accept the meeting **UUID**, which can contain `/` and must be double-URL-encoded). **Recommendation:** model a single `string` (or a small `MeetingId` value type with implicit conversion from `long`) everywhere in the C# surface rather than mirroring the spec's `long`/`string` split — safer and matches Zoom's actual documented behavior ("Double encode your UUID... if it begins with `/` or contains `//`").
4. **No shared error schema in the spec** — 400/401/403/404/429 responses only carry a Markdown `description`, no schema. Zoom's actual error body (undocumented in this spec but well known from their REST docs) is `{ "code": number, "message": string }`. Recommend hand-defining one `ZoomApiException`/`ZoomErrorResponse` DTO and using it uniformly rather than trying to generate it.
5. **No rate-limit response headers documented** in the spec (`429` is text-only, links to docs). Zoom does send `X-RateLimit-Category`, `X-RateLimit-Type`, `Retry-After` in practice — plan retry/backoff handling around real headers, not the spec.
6. **Pagination isn't 100% uniform**: most list endpoints use `next_page_token` + `page_size` (token-based), but a handful of older endpoints (H.323 devices, some registrant/participant lists) also expose `page_number`. A generic `IAsyncEnumerable<T>` paginator helper should support both, defaulting to token-based.

## 5. Recommendation for the .NET extension

**Don't hand-write 186 method signatures.** Given the spec exists and is reasonably well-formed (just verbose/inline), generate the client and hand-polish the auth/pagination/error layers:

- **Codegen**: [NSwag](https://github.com/RicoSuter/NSwag) or [Kiota](https://github.com/microsoft/kiota) against this file directly.
  - NSwag (`nswag openapi2csclient`) is the more mature choice for a "one big flat client class" and handles `allOf` flattening well; downside is it will generate ~186 near-duplicate small response DTOs unless you post-process.
  - Kiota generates a fluent, path-segment-based client (`client.Meetings[id].Registrants.GetAsync()`) which maps nicely to a `.AddZoomMeetingsClient()` DI extension and gives per-resource navigation, but needs .NET 6+ and its own runtime package.
  - Either way, **pre-process the spec** before feeding it in: fix the `meetingId` type inconsistency (rewrite all `integer` `meetingId` params to `string` in a local copy) so the generator doesn't emit two incompatible overload shapes for what's conceptually one identifier.
- **Hand-write** (generators won't get these right):
  - `DelegatingHandler` for bearer-token injection + refresh (Server-to-Server OAuth token fetch/cache, since the Meetings spec doesn't include the OAuth token endpoint).
  - Polly-based retry/backoff on `429`, honoring `Retry-After`.
  - A `ZoomApiException` with parsed `{code, message}` and the HTTP status, thrown from a shared response-handling path.
  - A small pagination helper (`IAsyncEnumerable<T>`) wrapping `next_page_token` loops, since generated clients usually leave you to loop manually.
- **Package shape**: `Microsoft.Extensions.DependencyInjection`-style extension, e.g. `services.AddZoomMeetingsClient(opts => { opts.AccountId = ...; opts.ClientId = ...; opts.ClientSecret = ...; })`, registering a typed `HttpClient` (via `IHttpClientFactory`) with the auth handler attached — this is presumably the ".NET extension" shape you meant (an `IServiceCollection` extension method + typed client), rather than a Visual Studio extension.
- **Scope down v1**: given 186 operations, consider generating everything but only *documenting/testing* the Meetings + Registrants + Recordings + Reports groups first (the ones most likely to be used), treating Devices/H.323/SIP/TSP as generated-but-unverified until needed.

## 6. Open questions for you

1. Target framework — .NET 8 (current LTS) or something else already in use in your other repos (I see `paypal-partner-gateway-dotnet`, `pesaflow4j`, `ecitizen-pesaflow-gateway-dotnet` as prior .NET/Java gateway projects)?
2. Codegen preference — NSwag (flat client) vs Kiota (fluent/DI-native) vs fully hand-rolled?
3. Full surface (all 186 ops) or scoped to Meetings/Registrants/Recordings/Reports first?
