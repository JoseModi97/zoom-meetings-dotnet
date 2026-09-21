using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ZoomMeetings.AspNetCore;

public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps a POST endpoint at <paramref name="pattern"/> that verifies Zoom's webhook signature,
    /// answers the endpoint.url_validation handshake automatically, and calls
    /// <paramref name="onEvent"/> with the parsed JSON payload for every other event. Responds 401 if
    /// the signature doesn't verify, 200 for everything else (Zoom expects a fast 2xx; do slow work
    /// out-of-band rather than inside <paramref name="onEvent"/>).
    /// </summary>
    public static IEndpointConventionBuilder MapZoomWebhook(
        this IEndpointRouteBuilder endpoints,
        string pattern,
        string secretToken,
        Func<JsonElement, HttpContext, Task> onEvent)
    {
        // Explicit Task<IResult> return type (not just "async (context) => ..."): Results.Unauthorized(),
        // Results.Ok(object), and Results.Ok() each return a different concrete IResult-implementing
        // type, so without an explicit target type the lambda has no single inferrable natural type,
        // which MapPost's Delegate parameter needs.
        return endpoints.MapPost(pattern, async Task<IResult> (HttpContext context) =>
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var rawBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            var signature = context.Request.Headers["x-zm-signature"].ToString();
            var timestamp = context.Request.Headers["x-zm-request-timestamp"].ToString();

            if (!ZoomWebhookHandler.VerifySignature(signature, timestamp, rawBody, secretToken))
            {
                return Results.Unauthorized();
            }

            using var document = JsonDocument.Parse(rawBody);
            var root = document.RootElement;

            var validationResponse = ZoomWebhookHandler.TryHandleUrlValidation(root, secretToken);
            if (validationResponse != null)
            {
                return Results.Ok(validationResponse);
            }

            await onEvent(root, context);
            return Results.Ok();
        });
    }
}
