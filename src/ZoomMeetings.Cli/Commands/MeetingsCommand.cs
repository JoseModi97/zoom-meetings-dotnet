using ZoomMeetings.Models;

namespace ZoomMeetings.Cli.Commands;

internal static class MeetingsCommand
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
                var request = new CreateMeetingRequest
                {
                    Topic = CliArgs.GetOption(rest, "--topic") ?? throw new ArgumentException("--topic is required"),
                    StartTime = TryParseDate(CliArgs.GetOption(rest, "--start-time")),
                    Duration = TryParseInt(CliArgs.GetOption(rest, "--duration")),
                    Agenda = CliArgs.GetOption(rest, "--agenda"),
                };
                var meeting = await client.CreateMeetingAsync(userId, request);
                CliOutput.WriteJson(meeting);
                break;
            }

            case "get":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                var meeting = await client.GetMeetingAsync(meetingId);
                CliOutput.WriteJson(meeting);
                break;
            }

            case "update":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                var request = new UpdateMeetingRequest
                {
                    Topic = CliArgs.GetOption(rest, "--topic"),
                    StartTime = TryParseDate(CliArgs.GetOption(rest, "--start-time")),
                    Duration = TryParseInt(CliArgs.GetOption(rest, "--duration")),
                    Agenda = CliArgs.GetOption(rest, "--agenda"),
                };
                await client.UpdateMeetingAsync(meetingId, request);
                CliOutput.WriteSuccess($"Meeting {meetingId} updated.");
                break;
            }

            case "delete":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                await client.DeleteMeetingAsync(meetingId);
                CliOutput.WriteSuccess($"Meeting {meetingId} deleted.");
                break;
            }

            case "list":
            {
                var userId = CliArgs.GetOption(rest, "--user-id") ?? "me";
                var result = await client.ListMeetingsAsync(userId);
                CliOutput.WriteJson(result);
                break;
            }

            default:
                throw new ArgumentException($"Unknown meetings action: '{action}'. Expected create|get|update|delete|list.");
        }
    }

    internal static DateTimeOffset? TryParseDate(string? value) =>
        value != null && DateTimeOffset.TryParse(value, out var parsed) ? parsed : null;

    internal static int? TryParseInt(string? value) =>
        value != null && int.TryParse(value, out var parsed) ? parsed : null;
}
