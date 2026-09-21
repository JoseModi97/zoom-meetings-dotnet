using System.Text.Json;
using ZoomMeetings.AspNetCore;
using Xunit;

namespace ZoomMeetings.Tests;

public class ZoomWebhookHandlerTests
{
    private const string SecretToken = "my-webhook-secret";

    [Fact]
    public void VerifySignature_ValidSignature_ReturnsTrue()
    {
        var timestamp = "1700000000";
        var body = """{"event":"meeting.started"}""";
        var signature = ZoomWebhookHandler.ComputeSignature(timestamp, body, SecretToken);

        Assert.True(ZoomWebhookHandler.VerifySignature(signature, timestamp, body, SecretToken));
    }

    [Fact]
    public void VerifySignature_TamperedBody_ReturnsFalse()
    {
        var timestamp = "1700000000";
        var originalBody = """{"event":"meeting.started"}""";
        var signature = ZoomWebhookHandler.ComputeSignature(timestamp, originalBody, SecretToken);

        var tamperedBody = """{"event":"meeting.ended"}""";

        Assert.False(ZoomWebhookHandler.VerifySignature(signature, timestamp, tamperedBody, SecretToken));
    }

    [Fact]
    public void VerifySignature_WrongSecret_ReturnsFalse()
    {
        var timestamp = "1700000000";
        var body = """{"event":"meeting.started"}""";
        var signature = ZoomWebhookHandler.ComputeSignature(timestamp, body, SecretToken);

        Assert.False(ZoomWebhookHandler.VerifySignature(signature, timestamp, body, "a-different-secret"));
    }

    [Theory]
    [InlineData(null, "1700000000")]
    [InlineData("v0=abc", null)]
    [InlineData("", "1700000000")]
    public void VerifySignature_MissingHeaders_ReturnsFalseWithoutThrowing(string? signature, string? timestamp)
    {
        Assert.False(ZoomWebhookHandler.VerifySignature(signature, timestamp, "{}", SecretToken));
    }

    [Fact]
    public void TryHandleUrlValidation_MatchingEvent_ReturnsPlainAndEncryptedToken()
    {
        var payload = JsonDocument.Parse("""
            {"event":"endpoint.url_validation","payload":{"plainToken":"qgg8vlvZRS6UYooatFL8Aw"}}
            """).RootElement;

        var result = ZoomWebhookHandler.TryHandleUrlValidation(payload, SecretToken);

        Assert.NotNull(result);

        var json = JsonSerializer.Serialize(result);
        using var parsed = JsonDocument.Parse(json);
        var plainToken = parsed.RootElement.GetProperty("plainToken").GetString();
        var encryptedToken = parsed.RootElement.GetProperty("encryptedToken").GetString();

        Assert.Equal("qgg8vlvZRS6UYooatFL8Aw", plainToken);
        Assert.False(string.IsNullOrEmpty(encryptedToken));
    }

    [Fact]
    public void TryHandleUrlValidation_OtherEvent_ReturnsNull()
    {
        var payload = JsonDocument.Parse("""
            {"event":"meeting.started","payload":{"object":{"id":123}}}
            """).RootElement;

        var result = ZoomWebhookHandler.TryHandleUrlValidation(payload, SecretToken);

        Assert.Null(result);
    }

    [Fact]
    public void TryHandleUrlValidation_MissingPlainToken_ReturnsNull()
    {
        var payload = JsonDocument.Parse("""
            {"event":"endpoint.url_validation","payload":{}}
            """).RootElement;

        var result = ZoomWebhookHandler.TryHandleUrlValidation(payload, SecretToken);

        Assert.Null(result);
    }
}
