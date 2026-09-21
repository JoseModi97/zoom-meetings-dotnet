using System.Text.Json.Nodes;

namespace ZoomMeetings.Cli;

/// <summary>
/// Loads a ZoomConfig for the CLI: environment variables first, then appsettings*.json walked up
/// from the current directory. Mirrors the config-loading approach used elsewhere in this ecosystem's
/// CLI tooling. The --profile flag (or ZOOM_PROFILE env var) selects between "Production" (default,
/// plain ZOOM_* vars / "Zoom" appsettings section) and any other named profile (ZOOM_{PROFILE}_* vars
/// / "Zoom:{Profile}" appsettings section) - since Zoom has no real sandbox API host, a profile is
/// really just a second Zoom account/Server-to-Server app used for testing.
/// </summary>
internal static class ZoomCliConfigLoader
{
    public static ZoomConfig LoadConfig(string? profile)
    {
        var isDefault = string.IsNullOrEmpty(profile) || string.Equals(profile, "production", StringComparison.OrdinalIgnoreCase);
        var envPrefix = isDefault ? "ZOOM" : $"ZOOM_{profile!.ToUpperInvariant()}";
        var section = isDefault ? "Zoom" : $"Zoom:{profile}";

        var config = new ZoomConfig
        {
            AccountId = Environment.GetEnvironmentVariable($"{envPrefix}_ACCOUNT_ID") ?? string.Empty,
            ClientId = Environment.GetEnvironmentVariable($"{envPrefix}_CLIENT_ID") ?? string.Empty,
            ClientSecret = Environment.GetEnvironmentVariable($"{envPrefix}_CLIENT_SECRET") ?? string.Empty,
        };

        if (!string.IsNullOrWhiteSpace(config.AccountId) && !string.IsNullOrWhiteSpace(config.ClientId) && !string.IsNullOrWhiteSpace(config.ClientSecret))
            return config;

        var current = Directory.GetCurrentDirectory();
        while (!string.IsNullOrWhiteSpace(current))
        {
            var devJson = Path.Combine(current, "appsettings.Development.json");
            var prodJson = Path.Combine(current, "appsettings.json");

            if (TryLoadFromJson(devJson, section, config) || TryLoadFromJson(prodJson, section, config))
                break;

            var parent = Directory.GetParent(current);
            if (parent == null || parent.FullName == current) break;
            current = parent.FullName;
        }

        return config;
    }

    private static bool TryLoadFromJson(string filePath, string sectionPath, ZoomConfig config)
    {
        if (!File.Exists(filePath)) return false;

        try
        {
            var root = JsonNode.Parse(File.ReadAllText(filePath))?.AsObject();
            if (root == null) return false;

            JsonObject? node = root;
            foreach (var segment in sectionPath.Split(':'))
            {
                node = node?[segment] as JsonObject;
                if (node == null) return false;
            }

            var accountId = node["AccountId"]?.ToString();
            var clientId = node["ClientId"]?.ToString();
            var clientSecret = node["ClientSecret"]?.ToString();

            var updated = false;
            if (string.IsNullOrWhiteSpace(config.AccountId) && !string.IsNullOrWhiteSpace(accountId)) { config.AccountId = accountId!; updated = true; }
            if (string.IsNullOrWhiteSpace(config.ClientId) && !string.IsNullOrWhiteSpace(clientId)) { config.ClientId = clientId!; updated = true; }
            if (string.IsNullOrWhiteSpace(config.ClientSecret) && !string.IsNullOrWhiteSpace(clientSecret)) { config.ClientSecret = clientSecret!; updated = true; }

            return updated && !string.IsNullOrWhiteSpace(config.AccountId);
        }
        catch
        {
            return false;
        }
    }
}
