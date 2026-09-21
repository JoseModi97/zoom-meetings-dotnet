using System.Net.Http;
using System.Text.Json;

namespace ZoomMeetings.Cli.Commands;

/// <summary>
/// `zoom-meetings raw &lt;METHOD&gt; &lt;path&gt; [--body &lt;json&gt;]` - reaches any of the ~186
/// operations in Zoom's Meetings API, not just the ones with a dedicated typed command. Mirrors
/// ZoomClient.CallAsync directly.
/// </summary>
internal static class RawCommand
{
    public static async Task ExecuteAsync(string[] args, string? profile)
    {
        if (args.Length < 2)
            throw new ArgumentException("Usage: zoom-meetings raw <METHOD> <path> [--body <json>]");

        var method = new HttpMethod(args[0].ToUpperInvariant());
        var path = args[1];
        var bodyJson = CliArgs.GetOption(args, "--body");

        object? body = bodyJson == null ? null : JsonSerializer.Deserialize<JsonElement>(bodyJson);

        using var client = new ZoomClient(ZoomCliConfigLoader.LoadConfig(profile));
        var response = await client.CallAsync<JsonElement>(method, path, body);
        CliOutput.WriteJson(response);
    }
}
