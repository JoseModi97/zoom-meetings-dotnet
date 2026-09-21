using System.Net.Http;

namespace ZoomMeetings.Internal;

/// <summary>
/// HttpMethod.Patch is a static property added in .NET 5+ and isn't available on netstandard2.0's
/// System.Net.Http surface, so this provides an equivalent that works on every target framework
/// this library supports.
/// </summary>
internal static class HttpMethods
{
    public static readonly HttpMethod Patch = new("PATCH");
}
