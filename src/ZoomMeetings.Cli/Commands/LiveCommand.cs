using ZoomMeetings.Models;

namespace ZoomMeetings.Cli.Commands;

internal static class LiveCommand
{
    public static async Task ExecuteAsync(string[] args, string? profile)
    {
        var action = args.Length > 0 ? args[0].ToLowerInvariant() : "help";
        var config = ZoomCliConfigLoader.LoadConfig(profile);
        using var client = new ZoomClient(config);

        switch (action)
        {
            case "chat-delete":
                var meetingId = CliArgs.GetOption(args, "--meeting-id")
                    ?? throw new ArgumentException("Missing required --meeting-id option.");
                var messageId = CliArgs.GetOption(args, "--message-id")
                    ?? throw new ArgumentException("Missing required --message-id option.");
                await client.DeleteLiveMeetingChatMessageAsync(meetingId, messageId);
                CliOutput.WriteSuccess($"Chat message '{messageId}' deleted from live meeting '{meetingId}'.");
                break;

            case "action":
            case "event":
                meetingId = CliArgs.GetOption(args, "--meeting-id")
                    ?? throw new ArgumentException("Missing required --meeting-id option.");
                var method = CliArgs.GetOption(args, "--method")
                    ?? throw new ArgumentException("Missing required --method option (e.g. participant.remove, room_system.callout, waiting_room.update_title_and_description).");
                var paramJson = CliArgs.GetOption(args, "--params") ?? "{}";
                var req = new InMeetingControlRequest
                {
                    Method = method,
                    Params = System.Text.Json.JsonSerializer.Deserialize<InMeetingControlParams>(paramJson)
                };
                await client.InMeetingControlAsync(meetingId, req);
                CliOutput.WriteSuccess($"Sent in-meeting control event '{method}' to meeting '{meetingId}'.");
                break;

            default:
                Console.WriteLine("Usage: zoom-meetings live <chat-delete|action> --meeting-id <id> [options]");
                break;
        }
    }
}
