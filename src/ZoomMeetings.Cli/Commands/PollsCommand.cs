using ZoomMeetings.Models;

namespace ZoomMeetings.Cli.Commands;

internal static class PollsCommand
{
    public static async Task ExecuteAsync(string[] args, string? profile)
    {
        var action = args.Length > 0 ? args[0] : string.Empty;
        var rest = args.Length > 1 ? args[1..] : Array.Empty<string>();

        using var client = new ZoomClient(ZoomCliConfigLoader.LoadConfig(profile));

        switch (action.ToLowerInvariant())
        {
            case "list":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                CliOutput.WriteJson(await client.ListPollsAsync(meetingId));
                break;
            }

            case "create":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                var title = CliArgs.GetOption(rest, "--title") ?? throw new ArgumentException("--title is required");
                CliOutput.WriteJson(await client.CreatePollAsync(meetingId, new Poll { Title = title }));
                break;
            }

            case "get":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                var pollId = CliArgs.GetOption(rest, "--poll-id") ?? throw new ArgumentException("--poll-id is required");
                CliOutput.WriteJson(await client.GetPollAsync(meetingId, pollId));
                break;
            }

            case "update":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                var pollId = CliArgs.GetOption(rest, "--poll-id") ?? throw new ArgumentException("--poll-id is required");
                var title = CliArgs.GetOption(rest, "--title") ?? throw new ArgumentException("--title is required");
                await client.UpdatePollAsync(meetingId, pollId, new Poll { Title = title });
                CliOutput.WriteSuccess($"Poll {pollId} updated.");
                break;
            }

            case "delete":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                var pollId = CliArgs.GetOption(rest, "--poll-id") ?? throw new ArgumentException("--poll-id is required");
                await client.DeletePollAsync(meetingId, pollId);
                CliOutput.WriteSuccess($"Poll {pollId} deleted.");
                break;
            }

            default:
                throw new ArgumentException($"Unknown polls action: '{action}'. Expected list|create|get|update|delete.");
        }
    }
}
