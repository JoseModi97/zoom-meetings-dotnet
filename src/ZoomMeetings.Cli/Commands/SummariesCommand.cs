namespace ZoomMeetings.Cli.Commands;

internal static class SummariesCommand
{
    public static async Task ExecuteAsync(string[] args, string? profile)
    {
        var action = args.Length > 0 ? args[0] : string.Empty;
        var rest = args.Length > 1 ? args[1..] : Array.Empty<string>();

        using var client = new ZoomClient(ZoomCliConfigLoader.LoadConfig(profile));

        switch (action.ToLowerInvariant())
        {
            case "get":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                CliOutput.WriteJson(await client.GetMeetingSummaryAsync(meetingId));
                break;
            }

            case "list":
            {
                CliOutput.WriteJson(await client.ListAccountMeetingSummariesAsync());
                break;
            }

            case "delete":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                await client.DeleteMeetingSummaryAsync(meetingId);
                CliOutput.WriteSuccess($"Summary for meeting {meetingId} deleted.");
                break;
            }

            default:
                throw new ArgumentException($"Unknown summaries action: '{action}'. Expected get|list|delete.");
        }
    }
}
