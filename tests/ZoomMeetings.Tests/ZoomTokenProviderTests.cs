using System.Net;
using System.Net.Http;
using ZoomMeetings.Internal;
using Xunit;

namespace ZoomMeetings.Tests;

public class ZoomTokenProviderTests
{
    private static ZoomConfig ValidConfig() => new()
    {
        AccountId = "acct-1",
        ClientId = "client-1",
        ClientSecret = "secret-1",
    };

    [Fact]
    public async Task GetAccessTokenAsync_ReturnsTokenFromResponse()
    {
        var handler = TestHttpMessageHandler.Json(HttpStatusCode.OK, """{"access_token":"tok-abc","token_type":"bearer","expires_in":3600}""");
        var provider = new ZoomTokenProvider(ValidConfig(), new HttpClient(handler));

        var token = await provider.GetAccessTokenAsync(CancellationToken.None);

        Assert.Equal("tok-abc", token);
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task GetAccessTokenAsync_CachesTokenAcrossCalls()
    {
        var handler = TestHttpMessageHandler.Json(HttpStatusCode.OK, """{"access_token":"tok-abc","token_type":"bearer","expires_in":3600}""");
        var provider = new ZoomTokenProvider(ValidConfig(), new HttpClient(handler));

        await provider.GetAccessTokenAsync(CancellationToken.None);
        await provider.GetAccessTokenAsync(CancellationToken.None);

        Assert.Single(handler.Requests); // second call served from cache, no second HTTP request
    }

    [Fact]
    public async Task GetAccessTokenAsync_RefetchesAfterExpiry()
    {
        var handler = TestHttpMessageHandler.Sequence(
            _ => Build("tok-1", 1), // expires in 1s; the 60s refresh skew makes the very next call refetch
            _ => Build("tok-2", 3600));
        var provider = new ZoomTokenProvider(ValidConfig(), new HttpClient(handler));

        var first = await provider.GetAccessTokenAsync(CancellationToken.None);
        var second = await provider.GetAccessTokenAsync(CancellationToken.None);

        Assert.Equal("tok-1", first);
        Assert.Equal("tok-2", second);
        Assert.Equal(2, handler.Requests.Count);
    }

    [Fact]
    public async Task GetAccessTokenAsync_MissingCredentials_Throws()
    {
        var handler = TestHttpMessageHandler.Json(HttpStatusCode.OK, "{}");
        var provider = new ZoomTokenProvider(new ZoomConfig(), new HttpClient(handler));

        await Assert.ThrowsAsync<InvalidOperationException>(() => provider.GetAccessTokenAsync(CancellationToken.None));
    }

    private static HttpResponseMessage Build(string token, int expiresIn) =>
        new(HttpStatusCode.OK)
        {
            Content = new StringContent($$"""{"access_token":"{{token}}","token_type":"bearer","expires_in":{{expiresIn}}}"""),
        };
}
