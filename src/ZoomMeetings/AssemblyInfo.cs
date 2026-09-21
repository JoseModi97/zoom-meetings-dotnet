using System.Runtime.CompilerServices;

// ZoomMeetings.AspNetCore needs to construct ZoomAuthHandler/ZoomTokenProvider directly when wiring
// up IHttpClientFactory's handler pipeline (see ServiceCollectionExtensions.cs).
[assembly: InternalsVisibleTo("ZoomMeetings.AspNetCore")]

// ZoomMeetings.Tests needs to unit test internal pieces (ZoomTokenProvider, SnakeCaseNamingPolicy,
// ZoomIdEncoding, ZoomErrorBody) directly rather than only through the public ZoomClient surface.
[assembly: InternalsVisibleTo("ZoomMeetings.Tests")]
