using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ZoomMeetings.Cli.Commands;

/// <summary>
/// Auto-detects the host platform (ASP.NET Core, Azure Functions, Blazor, Console, Docker, etc.)
/// and configures Zoom Server-to-Server OAuth credentials, saving into the appropriate config format
/// (appsettings.Development.json, local.settings.json, or .env) and optionally scaffolding starter code.
/// </summary>
internal static class SetupCommand
{
    public static async Task ExecuteAsync(string[] args, string? profile)
    {
        var isDefault = string.IsNullOrEmpty(profile) || string.Equals(profile, "production", StringComparison.OrdinalIgnoreCase);
        var profileName = isDefault ? "Production" : profile!;
        var currentDir = Directory.GetCurrentDirectory();

        // 1. Auto-detect host platform
        var detected = PlatformDetector.Detect(currentDir);

        Console.WriteLine($"=== Zoom Meetings Setup (Profile: {profileName}) ===");
        Console.WriteLine("Configures credentials for Zoom Server-to-Server OAuth app.\n");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("⚡ Auto-detected Environment:");
        Console.ResetColor();
        Console.WriteLine($"  Platform       : {detected.DisplayName}");
        Console.WriteLine($"  Target Config  : {detected.ConfigFileName}");
        Console.WriteLine($"  Terminal Shell : {detected.Shell}");
        if (!string.IsNullOrEmpty(detected.UserSecretsId))
        {
            Console.WriteLine($"  User Secrets   : {detected.UserSecretsId}");
        }
        Console.WriteLine();

        var accountId = CliArgs.GetOption(args, "--account-id");
        var clientId = CliArgs.GetOption(args, "--client-id");
        var clientSecret = CliArgs.GetOption(args, "--client-secret");
        var target = CliArgs.GetOption(args, "--target") ?? CliArgs.GetOption(args, "--file");
        var skipVerify = CliArgs.HasFlag(args, "--no-verify");
        var generateCode = CliArgs.HasFlag(args, "--generate-code") || CliArgs.HasFlag(args, "--code") || CliArgs.HasFlag(args, "--scaffold");

        if (string.IsNullOrWhiteSpace(accountId))
        {
            if (Console.IsInputRedirected)
            {
                throw new InvalidOperationException("Missing --account-id argument.");
            }
            Console.Write("Enter Zoom Account ID: ");
            accountId = Console.ReadLine()?.Trim();
        }

        if (string.IsNullOrWhiteSpace(clientId))
        {
            if (Console.IsInputRedirected)
            {
                throw new InvalidOperationException("Missing --client-id argument.");
            }
            Console.Write("Enter Zoom Client ID: ");
            clientId = Console.ReadLine()?.Trim();
        }

        if (string.IsNullOrWhiteSpace(clientSecret))
        {
            if (Console.IsInputRedirected)
            {
                throw new InvalidOperationException("Missing --client-secret argument.");
            }
            Console.Write("Enter Zoom Client Secret: ");
            clientSecret = Console.ReadLine()?.Trim();
        }

        if (string.IsNullOrWhiteSpace(accountId) || string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
        {
            throw new InvalidOperationException("Account ID, Client ID, and Client Secret are all required.");
        }

        // Determine destination file according to detected platform or explicit override
        string targetFilePath;
        ConfigFormat chosenFormat;

        if (string.IsNullOrWhiteSpace(target))
        {
            if (!Console.IsInputRedirected)
            {
                Console.Write($"Save target [{detected.ConfigFileName}]: ");
                var choice = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(choice))
                {
                    targetFilePath = Path.Combine(currentDir, detected.ConfigFileName);
                    chosenFormat = detected.Format;
                }
                else
                {
                    targetFilePath = Path.IsPathRooted(choice) ? choice : Path.Combine(currentDir, choice);
                    chosenFormat = choice.EndsWith(".env", StringComparison.OrdinalIgnoreCase)
                        ? ConfigFormat.DotEnv
                        : choice.EndsWith("local.settings.json", StringComparison.OrdinalIgnoreCase)
                            ? ConfigFormat.LocalSettingsJson
                            : ConfigFormat.Json;
                }
            }
            else
            {
                targetFilePath = Path.Combine(currentDir, detected.ConfigFileName);
                chosenFormat = detected.Format;
            }
        }
        else
        {
            if (target.Equals("env", StringComparison.OrdinalIgnoreCase) || target.EndsWith(".env", StringComparison.OrdinalIgnoreCase))
            {
                chosenFormat = ConfigFormat.DotEnv;
                targetFilePath = Path.IsPathRooted(target) ? target : Path.Combine(currentDir, target.EndsWith(".env") ? target : ".env");
            }
            else if (target.Equals("local.settings.json", StringComparison.OrdinalIgnoreCase) || target.Equals("local.settings", StringComparison.OrdinalIgnoreCase))
            {
                chosenFormat = ConfigFormat.LocalSettingsJson;
                targetFilePath = Path.Combine(currentDir, "local.settings.json");
            }
            else if (target.Equals("appsettings.development", StringComparison.OrdinalIgnoreCase) || target.Equals("appsettings.Development.json", StringComparison.OrdinalIgnoreCase))
            {
                chosenFormat = ConfigFormat.Json;
                targetFilePath = Path.Combine(currentDir, "appsettings.Development.json");
            }
            else
            {
                chosenFormat = ConfigFormat.Json;
                targetFilePath = Path.IsPathRooted(target) ? target : Path.Combine(currentDir, target.EndsWith(".json") ? target : $"{target}.json");
            }
        }

        // Save configuration according to detected/selected format
        switch (chosenFormat)
        {
            case ConfigFormat.LocalSettingsJson:
                SaveToLocalSettingsJson(targetFilePath, profileName, isDefault, accountId, clientId, clientSecret);
                break;

            case ConfigFormat.DotEnv:
                SaveToDotEnv(targetFilePath, profileName, isDefault, accountId, clientId, clientSecret);
                break;

            case ConfigFormat.Json:
            default:
                SaveToJson(targetFilePath, profileName, isDefault, accountId, clientId, clientSecret);
                break;
        }

        CliOutput.WriteSuccess($"\n[OK] Configuration successfully generated for {detected.DisplayName}:");
        Console.WriteLine($"     File: {targetFilePath}");

        // Also generate shell environment script for convenience
        var scriptName = detected.Shell == ShellType.PowerShell ? "zoom-env.ps1" : "zoom-env.sh";
        var scriptPath = Path.Combine(currentDir, scriptName);
        var scriptContent = CodeGenerator.GenerateShellScript(detected.Shell, accountId, clientId, clientSecret, profile);
        File.WriteAllText(scriptPath, scriptContent);
        Console.WriteLine($"     Shell Script: {scriptPath}");

        // Optionally scaffold starter code
        if (generateCode)
        {
            var codeFilePath = Path.Combine(currentDir, detected.SuggestedCodeFileName);
            var code = CodeGenerator.GenerateCode(detected.Platform);
            File.WriteAllText(codeFilePath, code);
            CliOutput.WriteSuccess($"[OK] Generated starter code for {detected.DisplayName}:");
            Console.WriteLine($"     File: {codeFilePath}");
        }

        // Live verification
        if (!skipVerify)
        {
            Console.WriteLine("\nVerifying credentials with Zoom Server-to-Server OAuth endpoint...");
            var config = new ZoomConfig
            {
                AccountId = accountId,
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            using var client = new ZoomClient(config);
            var user = await client.CallAsync<JsonElement>(HttpMethod.Get, "/users/me");

            var email = user.TryGetProperty("email", out var em) ? em.GetString() : "N/A";
            var userId = user.TryGetProperty("id", out var uid) ? uid.GetString() : "N/A";
            var accId = user.TryGetProperty("account_id", out var aid) ? aid.GetString() : accountId;

            CliOutput.WriteSuccess($"[OK] Successfully authenticated with Zoom!");
            Console.WriteLine($"     Account ID : {accId}");
            Console.WriteLine($"     User ID    : {userId}");
            Console.WriteLine($"     Email      : {email}");
        }

        // Summary of tools activated
        Console.WriteLine("\n==========================================================");
        CliOutput.WriteSuccess("✓ All 186 tools across 13 domains are activated & ready:");
        Console.WriteLine("==========================================================");
        Console.WriteLine("  1.  Meetings core            : 27 tools (schedule, join tokens, live stream, surveys, PAC)");
        Console.WriteLine("  2.  Registrants              : 8 tools  (single & batch registration, questions, approval)");
        Console.WriteLine("  3.  Polls & Quizzes          : 7 tools  (batch polls, questions, past results)");
        Console.WriteLine("  4.  Cloud Recordings         : 23 tools (downloads, AI transcripts, analytics, trash/recovery)");
        Console.WriteLine("  5.  Meeting Summaries        : 4 tools  (AI Companion summaries, action items, chapters)");
        Console.WriteLine("  6.  Reports & Analytics      : 24 tools (activities, billing, participants, audit logs)");
        Console.WriteLine("  7.  Webinars                 : 53 tools (panelists, branding, wallpaper/VB upload, tracking)");
        Console.WriteLine("  8.  Templates                : 2 tools  (meeting template listing and creation)");
        Console.WriteLine("  9.  Live Meeting Controls    : 4 tools  (in-meeting moderation, chat edit/delete, RTMS)");
        Console.WriteLine("  10. Tracking Fields          : 5 tools  (cost center, billing & departmental tagging)");
        Console.WriteLine("  11. SIP Phones               : 4 tools  (provisioning, listing, transport protocol)");
        Console.WriteLine("  12. Telephony (TSP)          : 8 tools  (audio conferencing, dial-in URLs, user accounts)");
        Console.WriteLine("  13. Devices & Zoom Rooms     : 17 tools (ZDM hardware, ZPA appliances, H.323/SIP rooms)");
        Console.WriteLine("----------------------------------------------------------");
        Console.WriteLine("Next steps:");
        Console.WriteLine("  • Explore all tools         : zoom-meetings tools");
        Console.WriteLine("  • Search tools by keyword   : zoom-meetings tools search <keyword>");
        Console.WriteLine("  • Scaffold starter code     : zoom-meetings generate");
        Console.WriteLine("  • Verify anytime            : zoom-meetings whoami");
        Console.WriteLine("  • List scheduled meetings   : zoom-meetings meetings list --user-id me");
        Console.WriteLine();
    }

    private static void SaveToJson(string filePath, string profileName, bool isDefault, string accountId, string clientId, string clientSecret)
    {
        JsonObject root;
        if (File.Exists(filePath))
        {
            try
            {
                var text = File.ReadAllText(filePath);
                root = JsonNode.Parse(text)?.AsObject() ?? new JsonObject();
            }
            catch
            {
                root = new JsonObject();
            }
        }
        else
        {
            root = new JsonObject();
        }

        JsonObject zoomSection;
        if (root.TryGetPropertyValue("Zoom", out var existingZoom) && existingZoom is JsonObject existingObj)
        {
            zoomSection = existingObj;
        }
        else
        {
            zoomSection = new JsonObject();
            root["Zoom"] = zoomSection;
        }

        if (isDefault)
        {
            zoomSection["AccountId"] = accountId;
            zoomSection["ClientId"] = clientId;
            zoomSection["ClientSecret"] = clientSecret;
        }
        else
        {
            JsonObject profileSection;
            if (zoomSection.TryGetPropertyValue(profileName, out var existingProf) && existingProf is JsonObject profObj)
            {
                profileSection = profObj;
            }
            else
            {
                profileSection = new JsonObject();
                zoomSection[profileName] = profileSection;
            }
            profileSection["AccountId"] = accountId;
            profileSection["ClientId"] = clientId;
            profileSection["ClientSecret"] = clientSecret;
        }

        var json = JsonSerializer.Serialize(root, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    private static void SaveToLocalSettingsJson(string filePath, string profileName, bool isDefault, string accountId, string clientId, string clientSecret)
    {
        JsonObject root;
        if (File.Exists(filePath))
        {
            try
            {
                var text = File.ReadAllText(filePath);
                root = JsonNode.Parse(text)?.AsObject() ?? new JsonObject();
            }
            catch
            {
                root = new JsonObject();
            }
        }
        else
        {
            root = new JsonObject();
            root["IsEncrypted"] = false;
        }

        JsonObject values;
        if (root.TryGetPropertyValue("Values", out var existingValues) && existingValues is JsonObject valObj)
        {
            values = valObj;
        }
        else
        {
            values = new JsonObject();
            root["Values"] = values;
        }

        var prefix = isDefault ? "Zoom" : $"Zoom:{profileName}";
        values[$"{prefix}:AccountId"] = accountId;
        values[$"{prefix}:ClientId"] = clientId;
        values[$"{prefix}:ClientSecret"] = clientSecret;

        // Also add __ format for Azure Functions environment binding
        var envPrefix = isDefault ? "Zoom" : $"Zoom__{profileName}";
        values[$"{envPrefix}__AccountId"] = accountId;
        values[$"{envPrefix}__ClientId"] = clientId;
        values[$"{envPrefix}__ClientSecret"] = clientSecret;

        var json = JsonSerializer.Serialize(root, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    private static void SaveToDotEnv(string filePath, string profileName, bool isDefault, string accountId, string clientId, string clientSecret)
    {
        var prefix = isDefault ? "ZOOM" : $"ZOOM_{profileName.ToUpperInvariant()}";
        var lines = File.Exists(filePath) ? File.ReadAllLines(filePath).ToList() : new List<string>();

        void Upsert(string key, string value)
        {
            var idx = lines.FindIndex(l =>
            {
                var trimmed = l.Trim();
                if (trimmed.StartsWith("#") || !trimmed.Contains('=')) return false;
                var k = trimmed.Substring(0, trimmed.IndexOf('=')).Trim();
                return k.Equals(key, StringComparison.OrdinalIgnoreCase);
            });

            var newLine = $"{key}={value}";
            if (idx >= 0)
                lines[idx] = newLine;
            else
                lines.Add(newLine);
        }

        Upsert($"{prefix}_ACCOUNT_ID", accountId);
        Upsert($"{prefix}_CLIENT_ID", clientId);
        Upsert($"{prefix}_CLIENT_SECRET", clientSecret);

        File.WriteAllLines(filePath, lines);
    }
}
