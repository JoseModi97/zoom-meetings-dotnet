namespace ZoomMeetings.Cli.Commands;

/// <summary>
/// Auto-detects the host platform (ASP.NET Core Minimal API, MVC, Blazor, Azure Functions, Console, Worker)
/// and generates the appropriate starter code, endpoints, or environment loaders.
/// </summary>
internal static class GenerateCommand
{
    public static Task ExecuteAsync(string[] args, string? profile)
    {
        var targetType = args.Length > 0 ? args[0].ToLowerInvariant() : "all";
        var platformOverride = CliArgs.GetOption(args, "--platform");
        var ns = CliArgs.GetOption(args, "--namespace") ?? "YourApp";
        var outDir = CliArgs.GetOption(args, "--output") ?? Directory.GetCurrentDirectory();

        var detected = PlatformDetector.Detect(outDir);

        var platform = detected.Platform;
        if (!string.IsNullOrEmpty(platformOverride))
        {
            platform = platformOverride.ToLowerInvariant() switch
            {
                "minimal-api" or "minimal" or "web" => PlatformType.AspNetCoreMinimalApi,
                "mvc" or "controller" => PlatformType.AspNetCoreMvc,
                "blazor" => PlatformType.AspNetCoreBlazor,
                "azure-functions" or "functions" or "func" => PlatformType.AzureFunctions,
                "worker" or "background" => PlatformType.DotNetWorker,
                "console" => PlatformType.DotNetConsole,
                "docker" or "env" => PlatformType.DockerOrPolyglot,
                _ => detected.Platform
            };
        }

        Console.WriteLine("\n=== Zoom Meetings Platform Code Generator ===");
        Console.WriteLine($"  Auto-detected Platform : {detected.DisplayName}");
        Console.WriteLine($"  Target Code File       : {detected.SuggestedCodeFileName}");
        Console.WriteLine($"  Description            : {detected.SuggestedCodeDescription}");
        Console.WriteLine($"  Terminal Shell         : {detected.Shell}\n");

        switch (targetType)
        {
            case "code":
            case "starter":
            case "endpoints":
                GenerateCodeFile(platform, detected.SuggestedCodeFileName, ns, outDir);
                break;

            case "env":
            case "script":
                GenerateEnvScript(detected.Shell, profile, outDir);
                break;

            case "all":
            default:
                GenerateCodeFile(platform, detected.SuggestedCodeFileName, ns, outDir);
                GenerateEnvScript(detected.Shell, profile, outDir);
                break;
        }

        Console.WriteLine();
        return Task.CompletedTask;
    }

    private static void GenerateCodeFile(PlatformType platform, string fileName, string ns, string outDir)
    {
        var filePath = Path.Combine(outDir, fileName);
        var code = CodeGenerator.GenerateCode(platform, ns);

        File.WriteAllText(filePath, code);
        CliOutput.WriteSuccess($"✓ Generated platform integration code: {filePath}");
        Console.WriteLine($"  Ready to compile with your {platform} project.");
    }

    private static void GenerateEnvScript(ShellType shell, string? profile, string outDir)
    {
        var scriptName = shell == ShellType.PowerShell ? "zoom-env.ps1" : "zoom-env.sh";
        var filePath = Path.Combine(outDir, scriptName);

        // Check if existing credentials exist to populate
        var config = ZoomCliConfigLoader.LoadConfig(profile);
        var accId = string.IsNullOrEmpty(config.AccountId) ? "YOUR_ACCOUNT_ID" : config.AccountId;
        var clientId = string.IsNullOrEmpty(config.ClientId) ? "YOUR_CLIENT_ID" : config.ClientId;
        var clientSecret = string.IsNullOrEmpty(config.ClientSecret) ? "YOUR_CLIENT_SECRET" : config.ClientSecret;

        var script = CodeGenerator.GenerateShellScript(shell, accId, clientId, clientSecret, profile);
        File.WriteAllText(filePath, script);

        CliOutput.WriteSuccess($"✓ Generated environment script: {filePath}");
        var runCmd = shell == ShellType.PowerShell ? ". .\\zoom-env.ps1" : "source ./zoom-env.sh";
        Console.WriteLine($"  Execute in current terminal session: {runCmd}");
    }
}
