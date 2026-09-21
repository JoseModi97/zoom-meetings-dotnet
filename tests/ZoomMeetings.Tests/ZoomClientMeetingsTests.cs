using System.Net;
using System.Net.Http;
using ZoomMeetings.Models;
using Xunit;

namespace ZoomMeetings.Tests;

public class ZoomClientMeetingsTests
{
    private static ZoomConfig ValidConfig() => new()
    {
        AccountId = "acct-1",
        ClientId = "client-1",
        ClientSecret = "secret-1",
    };

    [Fact]
    public async Task CreateMeetingAsync_HappyPath_ReturnsTypedMeeting()
    {
        var handler = new TestHttpMessageHandler(req =>
        {
            if (req.RequestUri!.Host == "zoom.us")
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"access_token":"tok","expires_in":3600}"""),
                };
            }

            Assert.Equal(HttpMethod.Post, req.Method);
            Assert.Contains("/users/me/meetings", req.RequestUri.PathAndQuery);

            return new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent("""{"id":123456789,"topic":"Sprint planning","host_email":"host@example.com"}"""),
            };
        });

        using var client = new ZoomClient(ValidConfig(), handler);

        var meeting = await client.CreateMeetingAsync("me", new CreateMeetingRequest { Topic = "Sprint planning" });

        Assert.NotNull(meeting);
        Assert.Equal(123456789, meeting!.Id);
        Assert.Equal("Sprint planning", meeting.Topic);
        Assert.Equal("host@example.com", meeting.HostEmail);
    }

    [Fact]
    public async Task GetMeetingAsync_400Response_ThrowsZoomApiExceptionWithParsedBody()
    {
        var handler = new TestHttpMessageHandler(req =>
        {
            if (req.RequestUri!.Host == "zoom.us")
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"access_token":"tok","expires_in":3600}"""),
                };
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("""{"code":300,"message":"Invalid meeting ID."}"""),
            };
        });

        using var client = new ZoomClient(ValidConfig(), handler);

        var ex = await Assert.ThrowsAsync<ZoomApiException>(() => client.GetMeetingAsync("bad-id"));

        Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
        Assert.Equal(300, ex.ZoomCode);
        Assert.Equal("Invalid meeting ID.", ex.ZoomMessage);
    }
}
