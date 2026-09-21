namespace ZoomMeetings.Cli;

public enum PlatformType
{
    AspNetCoreMinimalApi,
    AspNetCoreMvc,
    AspNetCoreBlazor,
    AzureFunctions,
    DotNetWorker,
    DotNetConsole,
    DockerOrPolyglot,
    Generic
}

public enum ConfigFormat
{
    Json,
    LocalSettingsJson,
    DotEnv
}

public enum ShellType
{
    PowerShell,
    Bash,
    Zsh,
    WindowsCmd
}

public record PlatformInfo(
    PlatformType Platform,
    string DisplayName,
    string ConfigFileName,
    ConfigFormat Format,
    string? CsprojPath,
    string? UserSecretsId,
    ShellType Shell,
    string SuggestedCodeFileName,
    string SuggestedCodeDescription
);

public static class PlatformDetector
{
    public static PlatformInfo Detect(string? startDirectory = null)
    {
        var dir = string.IsNullOrWhiteSpace(startDirectory) ? Directory.GetCurrentDirectory() : startDirectory;
        var shell = DetectShell();

        // 1. Walk up looking for project files
        var current = dir;
        string? foundCsproj = null;
        string? foundHostJson = null;
        string? foundPackageJson = null;
        string? foundDockerfile = null;
        string? foundAppSettings = null;
        string? foundLocalSettings = null;

        while (!string.IsNullOrWhiteSpace(current))
        {
            if (foundCsproj == null)
            {
                var csprojs = Directory.GetFiles(current, "*.csproj");
                if (csprojs.Length > 0) foundCsproj = csprojs[0];
            }

            if (foundHostJson == null && File.Exists(Path.Combine(current, "host.json")))
                foundHostJson = Path.Combine(current, "host.json");

            if (foundLocalSettings == null && File.Exists(Path.Combine(current, "local.settings.json")))
                foundLocalSettings = Path.Combine(current, "local.settings.json");

            if (foundAppSettings == null && (File.Exists(Path.Combine(current, "appsettings.Development.json")) || File.Exists(Path.Combine(current, "appsettings.json"))))
                foundAppSettings = File.Exists(Path.Combine(current, "appsettings.Development.json"))
                    ? Path.Combine(current, "appsettings.Development.json")
                    : Path.Combine(current, "appsettings.json");

            if (foundDockerfile == null && (File.Exists(Path.Combine(current, "Dockerfile")) || File.Exists(Path.Combine(current, "docker-compose.yml"))))
                foundDockerfile = File.Exists(Path.Combine(current, "Dockerfile")) ? Path.Combine(current, "Dockerfile") : Path.Combine(current, "docker-compose.yml");

            if (foundPackageJson == null && File.Exists(Path.Combine(current, "package.json")))
                foundPackageJson = Path.Combine(current, "package.json");

            var parent = Directory.GetParent(current);
            if (parent == null || parent.FullName == current) break;
            current = parent.FullName;
        }

        // 2. Evaluate project type
        if (foundCsproj != null)
        {
            var content = File.ReadAllText(foundCsproj);
            var userSecretsId = ExtractUserSecretsId(content);

            // Check if Azure Functions
            if (foundHostJson != null || content.Contains("Microsoft.Azure.Functions.Worker", StringComparison.OrdinalIgnoreCase))
            {
                return new PlatformInfo(
                    PlatformType.AzureFunctions,
                    "Azure Functions (.NET Isolated Worker)",
                    "local.settings.json",
                    ConfigFormat.LocalSettingsJson,
                    foundCsproj,
                    userSecretsId,
                    shell,
                    "ZoomMeetingFunctions.cs",
                    "Azure Functions HTTP triggers for listing and creating Zoom meetings"
                );
            }

            // Check if ASP.NET Core Web SDK
            if (content.Contains("Microsoft.NET.Sdk.Web", StringComparison.OrdinalIgnoreCase) || content.Contains("Microsoft.AspNetCore", StringComparison.OrdinalIgnoreCase))
            {
                var targetConfig = foundAppSettings != null ? Path.GetFileName(foundAppSettings) : "appsettings.Development.json";

                // Blazor check
                if (content.Contains("Microsoft.AspNetCore.Components", StringComparison.OrdinalIgnoreCase) ||
                    content.Contains("blazor", StringComparison.OrdinalIgnoreCase) ||
                    foundCsproj.Contains("blazor", StringComparison.OrdinalIgnoreCase) ||
                    (Directory.Exists(Path.GetDirectoryName(foundCsproj)!) && Directory.GetFiles(Path.GetDirectoryName(foundCsproj)!, "*.razor", SearchOption.AllDirectories).Length > 0))
                {
                    return new PlatformInfo(
                        PlatformType.AspNetCoreBlazor,
                        "ASP.NET Core Blazor",
                        targetConfig,
                        ConfigFormat.Json,
                        foundCsproj,
                        userSecretsId,
                        shell,
                        "ZoomDashboard.razor",
                        "Interactive Blazor component for Zoom meetings and recordings"
                    );
                }

                // MVC check
                if (Directory.Exists(Path.Combine(Path.GetDirectoryName(foundCsproj)!, "Controllers")) || content.Contains("AddControllers", StringComparison.OrdinalIgnoreCase))
                {
                    return new PlatformInfo(
                        PlatformType.AspNetCoreMvc,
                        "ASP.NET Core MVC",
                        targetConfig,
                        ConfigFormat.Json,
                        foundCsproj,
                        userSecretsId,
                        shell,
                        "ZoomMeetingsController.cs",
                        "ASP.NET Core MVC controller with Zoom meeting actions and webhook handler"
                    );
                }

                // Minimal API / Web API
                return new PlatformInfo(
                    PlatformType.AspNetCoreMinimalApi,
                    "ASP.NET Core Minimal API",
                    targetConfig,
                    ConfigFormat.Json,
                    foundCsproj,
                    userSecretsId,
                    shell,
                    "ZoomEndpoints.cs",
                    "ASP.NET Core Minimal API endpoint route group with DI and webhook signature verification"
                );
            }

            // Worker Service
            if (content.Contains("Microsoft.Extensions.Hosting", StringComparison.OrdinalIgnoreCase))
            {
                return new PlatformInfo(
                    PlatformType.DotNetWorker,
                    ".NET Background Worker Service",
                    foundAppSettings != null ? Path.GetFileName(foundAppSettings) : "appsettings.Development.json",
                    ConfigFormat.Json,
                    foundCsproj,
                    userSecretsId,
                    shell,
                    "ZoomWorker.cs",
                    "BackgroundService polling Zoom recordings and AI summaries"
                );
            }

            // Console or Class Library
            return new PlatformInfo(
                PlatformType.DotNetConsole,
                ".NET Console Application",
                foundAppSettings != null ? Path.GetFileName(foundAppSettings) : "appsettings.json",
                ConfigFormat.Json,
                foundCsproj,
                userSecretsId,
                shell,
                "ZoomQuickstart.cs",
                "Console runner demonstrating ZoomClient initialization and meeting creation"
            );
        }

        // 3. Fallbacks if no .csproj found
        if (foundHostJson != null)
        {
            return new PlatformInfo(
                PlatformType.AzureFunctions,
                "Azure Functions",
                "local.settings.json",
                ConfigFormat.LocalSettingsJson,
                null,
                null,
                shell,
                "ZoomMeetingFunctions.cs",
                "Azure Functions HTTP triggers for Zoom"
            );
        }

        if (foundAppSettings != null)
        {
            return new PlatformInfo(
                PlatformType.AspNetCoreMinimalApi,
                "ASP.NET Core Application",
                Path.GetFileName(foundAppSettings),
                ConfigFormat.Json,
                null,
                null,
                shell,
                "ZoomEndpoints.cs",
                "ASP.NET Core Minimal API endpoints"
            );
        }

        if (foundDockerfile != null || foundPackageJson != null)
        {
            return new PlatformInfo(
                PlatformType.DockerOrPolyglot,
                "Container / Polyglot Environment",
                ".env",
                ConfigFormat.DotEnv,
                null,
                null,
                shell,
                shell == ShellType.PowerShell ? "zoom-env.ps1" : "zoom-env.sh",
                "Environment variable loader script for Zoom credentials"
            );
        }

        return new PlatformInfo(
            PlatformType.Generic,
            "Standard Environment",
            ".env",
            ConfigFormat.DotEnv,
            null,
            null,
            shell,
            shell == ShellType.PowerShell ? "zoom-env.ps1" : "zoom-env.sh",
            "Environment variable loader script for Zoom credentials"
        );
    }

    private static ShellType DetectShell()
    {
        var shellEnv = Environment.GetEnvironmentVariable("SHELL")?.ToLowerInvariant();
        if (!string.IsNullOrEmpty(shellEnv))
        {
            if (shellEnv.Contains("zsh")) return ShellType.Zsh;
            if (shellEnv.Contains("bash")) return ShellType.Bash;
        }

        if (OperatingSystem.IsWindows())
        {
            var psModule = Environment.GetEnvironmentVariable("PSModulePath");
            if (!string.IsNullOrEmpty(psModule)) return ShellType.PowerShell;
            return ShellType.PowerShell; // Default modern Windows terminal
        }

        if (OperatingSystem.IsMacOS()) return ShellType.Zsh;
        return ShellType.Bash;
    }

    private static string? ExtractUserSecretsId(string csprojContent)
    {
        var startTag = "<UserSecretsId>";
        var endTag = "</UserSecretsId>";
        var start = csprojContent.IndexOf(startTag, StringComparison.OrdinalIgnoreCase);
        if (start < 0) return null;
        var end = csprojContent.IndexOf(endTag, start, StringComparison.OrdinalIgnoreCase);
        if (end < 0) return null;
        return csprojContent.Substring(start + startTag.Length, end - start - startTag.Length).Trim();
    }
}
