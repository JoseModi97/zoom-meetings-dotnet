using ZoomMeetings.Models;

namespace ZoomMeetings.Cli.Commands;

internal static class TrackingFieldsCommand
{
    public static async Task ExecuteAsync(string[] args, string? profile)
    {
        var action = args.Length > 0 ? args[0].ToLowerInvariant() : "list";
        var config = ZoomCliConfigLoader.LoadConfig(profile);
        using var client = new ZoomClient(config);

        switch (action)
        {
            case "list":
                var list = await client.ListTrackingFieldsAsync();
                CliOutput.WriteJson(list);
                break;

            case "get":
                var fieldId = CliArgs.GetOption(args, "--field-id")
                    ?? throw new ArgumentException("Missing required --field-id option.");
                var field = await client.GetTrackingFieldAsync(fieldId);
                CliOutput.WriteJson(field);
                break;

            case "create":
                var fieldName = CliArgs.GetOption(args, "--field")
                    ?? throw new ArgumentException("Missing required --field option.");
                var req = new CreateTrackingFieldRequest
                {
                    Field = fieldName,
                    Required = CliArgs.HasFlag(args, "--required"),
                    Visible = CliArgs.HasFlag(args, "--visible")
                };
                var created = await client.CreateTrackingFieldAsync(req);
                CliOutput.WriteSuccess($"Tracking field '{fieldName}' created.");
                CliOutput.WriteJson(created);
                break;

            case "delete":
                fieldId = CliArgs.GetOption(args, "--field-id")
                    ?? throw new ArgumentException("Missing required --field-id option.");
                await client.DeleteTrackingFieldAsync(fieldId);
                CliOutput.WriteSuccess($"Tracking field '{fieldId}' deleted.");
                break;

            default:
                Console.WriteLine("Usage: zoom-meetings tracking-fields <list|get|create|delete> [options]");
                break;
        }
    }
}
