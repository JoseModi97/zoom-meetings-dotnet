using ZoomMeetings.Models;

namespace ZoomMeetings.Cli.Commands;

internal static class WebinarsCommand
{
    public static async Task ExecuteAsync(string[] args, string? profile)
    {
        var action = args.Length > 0 ? args[0] : string.Empty;
        var rest = args.Length > 1 ? args[1..] : Array.Empty<string>();

        using var client = new ZoomClient(ZoomCliConfigLoader.LoadConfig(profile));

        switch (action.ToLowerInvariant())
        {
            case "create":
            {
                var userId = CliArgs.GetOption(rest, "--user-id") ?? "me";
                var request = new CreateWebinarRequest
                {
                    Topic = CliArgs.GetOption(rest, "--topic") ?? throw new ArgumentException("--topic is required"),
                    StartTime = MeetingsCommand.TryParseDate(CliArgs.GetOption(rest, "--start-time")),
                    Duration = MeetingsCommand.TryParseInt(CliArgs.GetOption(rest, "--duration")),
                    Agenda = CliArgs.GetOption(rest, "--agenda"),
                };
                CliOutput.WriteJson(await client.CreateWebinarAsync(userId, request));
                break;
            }

            case "get":
            {
                var webinarId = CliArgs.GetOption(rest, "--webinar-id") ?? throw new ArgumentException("--webinar-id is required");
                CliOutput.WriteJson(await client.GetWebinarAsync(webinarId));
                break;
            }

            case "update":
            {
                var webinarId = CliArgs.GetOption(rest, "--webinar-id") ?? throw new ArgumentException("--webinar-id is required");
                var request = new UpdateWebinarRequest
                {
                    Topic = CliArgs.GetOption(rest, "--topic"),
                    StartTime = MeetingsCommand.TryParseDate(CliArgs.GetOption(rest, "--start-time")),
                    Duration = MeetingsCommand.TryParseInt(CliArgs.GetOption(rest, "--duration")),
                };
                await client.UpdateWebinarAsync(webinarId, request);
                CliOutput.WriteSuccess($"Webinar {webinarId} updated.");
                break;
            }

            case "delete":
            {
                var webinarId = CliArgs.GetOption(rest, "--webinar-id") ?? throw new ArgumentException("--webinar-id is required");
                await client.DeleteWebinarAsync(webinarId);
                CliOutput.WriteSuccess($"Webinar {webinarId} deleted.");
                break;
            }

            case "list":
            {
                var userId = CliArgs.GetOption(rest, "--user-id") ?? "me";
                CliOutput.WriteJson(await client.ListWebinarsAsync(userId));
                break;
            }

            default:
                throw new ArgumentException($"Unknown webinars action: '{action}'. Expected create|get|update|delete|list.");
        }
    }
}
