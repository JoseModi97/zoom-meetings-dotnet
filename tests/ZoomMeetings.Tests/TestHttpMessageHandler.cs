using System.Net;
using System.Net.Http;
using System.Text;

namespace ZoomMeetings.Tests;

/// <summary>
/// A minimal fake HttpMessageHandler for testing HTTP-calling code without a real network call.
/// The reference repo this library's structure was modeled on has no HTTP-mocking example (its
/// "auth" is HMAC-signed form fields, not real outbound OAuth calls), so this was built fresh.
/// </summary>
public sealed class TestHttpMessageHandler : HttpMessageHandler
{
    public List<HttpRequestMessage> Requests { get; } = new();

    private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;

    public TestHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
    {
        _responder = responder;
    }

    public static TestHttpMessageHandler Json(HttpStatusCode statusCode, string json) =>
        new(_ => new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        });

    /// <summary>Returns a handler that replays responses in order, repeating the last one once exhausted.</summary>
    public static TestHttpMessageHandler Sequence(params Func<HttpRequestMessage, HttpResponseMessage>[] responders)
    {
        var index = 0;
        return new TestHttpMessageHandler(req =>
        {
            var responder = responders[Math.Min(index, responders.Length - 1)];
            index++;
            return responder(req);
        });
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add(request);
        return Task.FromResult(_responder(request));
    }
}
