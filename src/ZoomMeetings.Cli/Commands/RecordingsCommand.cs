namespace ZoomMeetings.Cli.Commands;

internal static class RecordingsCommand
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
                CliOutput.WriteJson(await client.GetMeetingRecordingsAsync(meetingId));
                break;
            }

            case "delete":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                var recordingAction = CliArgs.GetOption(rest, "--action");
                await client.DeleteMeetingRecordingsAsync(meetingId, recordingAction);
                CliOutput.WriteSuccess($"Recordings for meeting {meetingId} deleted.");
                break;
            }

            case "get-settings":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                CliOutput.WriteJson(await client.GetRecordingSettingsAsync(meetingId));
                break;
            }

            case "update-settings":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                var shareRecording = CliArgs.GetOption(rest, "--share-recording");
                await client.UpdateRecordingSettingsAsync(meetingId, new Models.RecordingSettings { ShareRecording = shareRecording });
                CliOutput.WriteSuccess($"Recording settings for meeting {meetingId} updated.");
                break;
            }

            default:
                throw new ArgumentException($"Unknown recordings action: '{action}'. Expected get|delete|get-settings|update-settings.");
        }
    }
}
