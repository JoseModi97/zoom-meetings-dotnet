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
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "0.2.0";
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
                case "setup":
                    await SetupCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "generate":
                    await GenerateCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "tools":
                    await ToolsCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

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

                case "templates":
                    await TemplatesCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "tracking-fields":
                case "trackingfields":
                    await TrackingFieldsCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "devices":
                    await DevicesCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "sip":
                    await SipCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "tsp":
                    await TspCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "live":
                    await LiveCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                case "whoami":
                    await WhoAmICommand.ExecuteAsync(profile);
                    return 0;

                case "raw":
                    await RawCommand.ExecuteAsync(resourceArgs, profile);
                    return 0;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Unknown command or resource: '{resource}'\n");
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
  zoom-meetings <command|resource> [action] [options] [--profile <name>]
  dotnet zoom-meetings <command|resource> [action] [options] [--profile <name>]

Setup & Discovery:
  setup          Auto-detect platform (ASP.NET Core, Functions, Blazor, Console, Docker) & configure credentials
                 Options: [--account-id <id>] [--client-id <id>] [--client-secret <secret>] [--target <file>] [--generate-code]
  generate       Auto-detect platform and generate integration boilerplate, endpoints, and environment loaders
                 Subcommands: all | code | env | endpoints  [--platform <type>] [--namespace <ns>]
  tools          Catalog, inspect, search, and verify all 186 tools across 13 domains
                 Subcommands: all | <domain> | search <keyword> | verify | json
  whoami         Verify Server-to-Server OAuth credentials and print account details

13 Tool Domains (100% Typed Coverage):
  meetings         create | get | update | delete | list
  registrants      list | add | get | delete | status
  polls            list | create | get | update | delete
  recordings       get | delete | get-settings | update-settings
  summaries        get | list | delete  (AI Companion meeting summaries)
  reports          detail | participants
  webinars         create | get | update | delete | list
  templates        list | create
  tracking-fields  list | get | create | delete
  devices          list | get | groups | h323-list | h323-create
  sip              list | enable | delete
  tsp              get-account | get-user
  live             chat-delete | action

Escape Hatch:
  raw            <METHOD> <path> [--body <json>]   e.g. raw GET /users/me

Global options:
  --profile, -p <name>   Selects credentials for a named environment. Default: Production.
  --help, -h             Show this help message
  --version, -v          Show version

Credentials:
  Configured via 'zoom-meetings setup' into appsettings.json or .env:
  ZOOM_ACCOUNT_ID / ZOOM_CLIENT_ID / ZOOM_CLIENT_SECRET              (Production / default profile)
  ZOOM_<PROFILE>_ACCOUNT_ID / _CLIENT_ID / _CLIENT_SECRET            (any other --profile <name>)

Examples:
  zoom-meetings setup                                      # Interactive setup wizard
  zoom-meetings tools                                      # Overview of all 13 tool domains
  zoom-meetings tools all                                  # Catalog all 186 tools
  zoom-meetings tools search transcript                    # Search tools by keyword
  zoom-meetings tools recordings                           # List all recording/archiving tools
  zoom-meetings whoami                                     # Verify active credentials
  zoom-meetings meetings list --user-id me
  zoom-meetings meetings create --user-id me --topic ""Sprint planning"" --start-time 2026-10-01T09:00:00Z
  zoom-meetings raw GET /users/me
");
    }
}
