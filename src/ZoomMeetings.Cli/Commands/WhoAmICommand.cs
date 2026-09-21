using System.Net.Http;
using System.Text.Json;

namespace ZoomMeetings.Cli.Commands;

/// <summary>
/// Verifies the selected profile's credentials work by fetching the token and calling GET /users/me
/// (Zoom's Users API, not the Meetings API this library wraps - but the same OAuth token/base URL
/// serve both, and it's the simplest reliable "is my Server-to-Server app configured correctly" check).
/// </summary>
internal static class WhoAmICommand
{
    public static async Task ExecuteAsync(string? profile)
    {
        var config = ZoomCliConfigLoader.LoadConfig(profile);
        using var client = new ZoomClient(config);

        var user = await client.CallAsync<JsonElement>(HttpMethod.Get, "/users/me");

        CliOutput.WriteSuccess($"Credentials OK for profile '{profile ?? "Production"}' (account {config.AccountId}).");
        if (user is JsonElement element)
        {
            CliOutput.WriteJson(new
            {
                id = element.TryGetProperty("id", out var id) ? id.GetString() : null,
                email = element.TryGetProperty("email", out var email) ? email.GetString() : null,
                account_id = element.TryGetProperty("account_id", out var accountId) ? accountId.GetString() : null,
                type = element.TryGetProperty("type", out var type) ? type.GetInt32() : (int?)null,
            });
        }
    }
}
