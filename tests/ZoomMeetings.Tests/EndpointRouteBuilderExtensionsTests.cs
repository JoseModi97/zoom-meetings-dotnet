using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZoomMeetings.AspNetCore;
using Xunit;

namespace ZoomMeetings.Tests;

public class EndpointRouteBuilderExtensionsTests
{
    private const string SecretToken = "endpoint-test-secret";
    private const string Timestamp = "1700000000";

    private static async Task<IHost> StartHostAsync(Func<JsonElement, Microsoft.AspNetCore.Http.HttpContext, Task> onEvent) =>
        await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseTestServer();
                webBuilder.ConfigureServices(services => services.AddRouting());
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapZoomWebhook("/webhooks/zoom", SecretToken, onEvent));
                });
            })
            .StartAsync();

    private static HttpRequestMessage SignedRequest(string body)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/webhooks/zoom")
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };
        request.Headers.Add("x-zm-signature", ZoomWebhookHandler.ComputeSignature(Timestamp, body, SecretToken));
        request.Headers.Add("x-zm-request-timestamp", Timestamp);
        return request;
    }

    [Fact]
    public async Task MapZoomWebhook_UrlValidationEvent_RespondsWithPlainAndEncryptedToken()
    {
        using var host = await StartHostAsync((_, _) => Task.CompletedTask);
        using var client = host.GetTestClient();

        var body = """{"event":"endpoint.url_validation","payload":{"plainToken":"abc123"}}""";
        using var response = await client.SendAsync(SignedRequest(body));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("abc123", json.RootElement.GetProperty("plainToken").GetString());
        Assert.False(string.IsNullOrEmpty(json.RootElement.GetProperty("encryptedToken").GetString()));
    }

    [Fact]
    public async Task MapZoomWebhook_InvalidSignature_Returns401AndDoesNotInvokeOnEvent()
    {
        var invoked = false;
        using var host = await StartHostAsync((_, _) => { invoked = true; return Task.CompletedTask; });
        using var client = host.GetTestClient();

        var body = """{"event":"meeting.started"}""";
        using var request = new HttpRequestMessage(HttpMethod.Post, "/webhooks/zoom")
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };
        request.Headers.Add("x-zm-signature", "v0=not-the-right-signature");
        request.Headers.Add("x-zm-request-timestamp", Timestamp);

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.False(invoked);
    }

    [Fact]
    public async Task MapZoomWebhook_OtherEvent_InvokesOnEventWithParsedPayloadAndReturns200()
    {
        JsonElement? received = null;
        using var host = await StartHostAsync((payload, _) =>
        {
            received = payload.Clone();
            return Task.CompletedTask;
        });
        using var client = host.GetTestClient();

        var body = """{"event":"meeting.started","payload":{"object":{"id":123456789}}}""";
        using var response = await client.SendAsync(SignedRequest(body));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(received);
        Assert.Equal("meeting.started", received!.Value.GetProperty("event").GetString());
        Assert.Equal(123456789, received.Value.GetProperty("payload").GetProperty("object").GetProperty("id").GetInt64());
    }
}
