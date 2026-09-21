namespace ZoomMeetings.Cli.Commands;

internal static class ReportsCommand
{
    public static async Task ExecuteAsync(string[] args, string? profile)
    {
        var action = args.Length > 0 ? args[0] : string.Empty;
        var rest = args.Length > 1 ? args[1..] : Array.Empty<string>();

        using var client = new ZoomClient(ZoomCliConfigLoader.LoadConfig(profile));

        switch (action.ToLowerInvariant())
        {
            case "detail":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                CliOutput.WriteJson(await client.GetMeetingReportDetailAsync(meetingId));
                break;
            }

            case "participants":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                CliOutput.WriteJson(await client.GetMeetingReportParticipantsAsync(meetingId));
                break;
            }

            default:
                throw new ArgumentException($"Unknown reports action: '{action}'. Expected detail|participants.");
        }
    }
}
