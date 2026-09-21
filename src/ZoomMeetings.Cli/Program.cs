using System.Reflection;
using ZoomMeetings.Cli.Commands;

namespace ZoomMeetings.Cli;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        if (args.Length > 0 && args[0] is "--help" or "-h" or "help")
        {
            PrintHelp();
            return 0;
        }

        if (args.Length > 0 && args[0] is "--version" or "-v" or "version")
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "0.1.0";
            Console.WriteLine($"zoom-meetings v{version}");
            return 0;
        }

        var (profile, rest) = CliArgs.ExtractProfile(args);
        var resource = rest.Length > 0 ? rest[0] : string.Empty;
        var resourceArgs = rest.Length > 1 ? rest[1..] : Array.Empty<string>();

        try
        {
            switch (resource.ToLowerInvariant())
            {
                case "meetings":
                    await MeetingsCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "registrants":
                    await RegistrantsCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "polls":
                    await PollsCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "recordings":
                    await RecordingsCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "summaries":
                    await SummariesCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "reports":
                    await ReportsCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "webinars":
                    await WebinarsCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "whoami":
                    await WhoAmICommand.ExecuteAsync(profile);
                    return 0;

                case "raw":
                    await RawCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Unknown resource: '{resource}'\n");
                    Console.ResetColor();
                    PrintHelp();
                    return 1;
            }
        }
        catch (ZoomApiException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nZoom API error: {ex.Message}");
            Console.ResetColor();
            return 1;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nError: {ex.Message}");
            Console.ResetColor();
            return 1;
        }
    }

    public static void PrintHelp()
    {
        Console.WriteLine(@"
Usage:
  zoom-meetings <resource> <action> [options] [--profile <name>]
  dotnet zoom-meetings <resource> <action> [options] [--profile <name>]

Resources:
  meetings       create | get | update | delete | list
  registrants    list | add | get | delete | status
  polls          list | create | get | update | delete
  recordings     get | delete | get-settings | update-settings
  summaries      get | list | delete
  reports        detail | participants
  webinars       create | get | update | delete | list
  whoami         Verify credentials and print the account they resolve to
  raw            <METHOD> <path> [--body <json>]   e.g. raw GET /meetings/123
  help, --help   Show this help message
  --version, -v  Show version

Global options:
  --profile, -p <name>   Selects credentials for a named environment (see below). Default: Production.

Credentials (env vars, or appsettings.json under a matching section - see README):
  ZOOM_ACCOUNT_ID / ZOOM_CLIENT_ID / ZOOM_CLIENT_SECRET              (Production / default profile)
  ZOOM_<PROFILE>_ACCOUNT_ID / _CLIENT_ID / _CLIENT_SECRET            (any other --profile <name>)

Examples:
  zoom-meetings whoami
  zoom-meetings meetings list --user-id me
  zoom-meetings meetings create --user-id me --topic ""Sprint planning"" --start-time 2026-10-01T09:00:00Z
  zoom-meetings registrants list --meeting-id 123456789
  zoom-meetings raw GET /meetings/123456789/batch_polls --profile Sandbox
");
    }
}
