using System.Net;
using System.Net.Http;
using ZoomMeetings.Internal;
using Xunit;

namespace ZoomMeetings.Tests;

public class ZoomAuthHandlerTests
{
    [Fact]
    public async Task SendAsync_AttachesBearerTokenFromProvider()
    {
        var tokenHandler = TestHttpMessageHandler.Json(HttpStatusCode.OK, """{"access_token":"tok-xyz","expires_in":3600}""");
        var tokenProvider = new ZoomTokenProvider(
            new ZoomConfig { AccountId = "a", ClientId = "b", ClientSecret = "c" },
            new HttpClient(tokenHandler));

        HttpRequestMessage? capturedRequest = null;
        var innerHandler = new TestHttpMessageHandler(req =>
        {
            capturedRequest = req;
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{}") };
        });

        var authHandler = new ZoomAuthHandler(tokenProvider) { InnerHandler = innerHandler };
        using var client = new HttpClient(authHandler);

        await client.GetAsync("https://api.zoom.us/v2/meetings/123");

        Assert.NotNull(capturedRequest);
        Assert.Equal("Bearer", capturedRequest!.Headers.Authorization?.Scheme);
        Assert.Equal("tok-xyz", capturedRequest.Headers.Authorization?.Parameter);
    }

    [Fact]
    public async Task SendAsync_On401_InvalidatesTokenAndRetriesOnce()
    {
        var tokenHandler = TestHttpMessageHandler.Sequence(
            _ => Json("tok-1"),
            _ => Json("tok-2"));
        var tokenProvider = new ZoomTokenProvider(
            new ZoomConfig { AccountId = "a", ClientId = "b", ClientSecret = "c" },
            new HttpClient(tokenHandler));

        var attempts = 0;
        var innerHandler = new TestHttpMessageHandler(req =>
        {
            attempts++;
            return attempts == 1
                ? new HttpResponseMessage(HttpStatusCode.Unauthorized) { Content = new StringContent("{}") }
                : new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{}") };
        });

        var authHandler = new ZoomAuthHandler(tokenProvider) { InnerHandler = innerHandler };
        using var client = new HttpClient(authHandler);

        var response = await client.GetAsync("https://api.zoom.us/v2/meetings/123");

        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(2, attempts);
        Assert.Equal(2, tokenHandler.Requests.Count); // token fetched once, then re-fetched after the 401
    }

    private static HttpResponseMessage Json(string token) =>
        new(HttpStatusCode.OK) { Content = new StringContent($$"""{"access_token":"{{token}}","expires_in":3600}""") };
}
