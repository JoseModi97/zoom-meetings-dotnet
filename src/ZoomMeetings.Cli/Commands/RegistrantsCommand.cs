using ZoomMeetings.Models;

namespace ZoomMeetings.Cli.Commands;

internal static class RegistrantsCommand
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
                var result = await client.ListRegistrantsAsync(meetingId);
                CliOutput.WriteJson(result);
                break;
            }

            case "add":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                var request = new AddRegistrantRequest
                {
                    Email = CliArgs.GetOption(rest, "--email") ?? throw new ArgumentException("--email is required"),
                    FirstName = CliArgs.GetOption(rest, "--first-name") ?? throw new ArgumentException("--first-name is required"),
                    LastName = CliArgs.GetOption(rest, "--last-name"),
                };
                var result = await client.AddRegistrantAsync(meetingId, request);
                CliOutput.WriteJson(result);
                break;
            }

            case "get":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                var registrantId = CliArgs.GetOption(rest, "--registrant-id") ?? throw new ArgumentException("--registrant-id is required");
                var result = await client.GetRegistrantAsync(meetingId, registrantId);
                CliOutput.WriteJson(result);
                break;
            }

            case "delete":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                var registrantId = CliArgs.GetOption(rest, "--registrant-id") ?? throw new ArgumentException("--registrant-id is required");
                await client.DeleteRegistrantAsync(meetingId, registrantId);
                CliOutput.WriteSuccess($"Registrant {registrantId} removed from meeting {meetingId}.");
                break;
            }

            case "status":
            {
                var meetingId = CliArgs.GetOption(rest, "--meeting-id") ?? throw new ArgumentException("--meeting-id is required");
                var registrantAction = CliArgs.GetOption(rest, "--action") ?? throw new ArgumentException("--action is required (approve|deny|cancel)");
                var registrantId = CliArgs.GetOption(rest, "--registrant-id") ?? throw new ArgumentException("--registrant-id is required");
                await client.UpdateRegistrantStatusAsync(meetingId, registrantAction, new[] { registrantId });
                CliOutput.WriteSuccess($"Registrant {registrantId} status set to '{registrantAction}'.");
                break;
            }

            default:
                throw new ArgumentException($"Unknown registrants action: '{action}'. Expected list|add|get|delete|status.");
        }
    }
}
