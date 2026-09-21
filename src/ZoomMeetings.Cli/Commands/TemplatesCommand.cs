using ZoomMeetings.Models;

namespace ZoomMeetings.Cli.Commands;

internal static class TemplatesCommand
{
    public static async Task ExecuteAsync(string[] args, string? profile)
    {
        var action = args.Length > 0 ? args[0].ToLowerInvariant() : "list";
        var config = ZoomCliConfigLoader.LoadConfig(profile);
        using var client = new ZoomClient(config);

        switch (action)
        {
            case "list":
                var userId = CliArgs.GetOption(args, "--user-id") ?? "me";
                var templates = await client.ListMeetingTemplatesAsync(userId);
                CliOutput.WriteJson(templates);
                break;

            case "create":
                userId = CliArgs.GetOption(args, "--user-id") ?? "me";
                var meetingId = CliArgs.GetOption(args, "--meeting-id")
                    ?? throw new ArgumentException("Missing required --meeting-id option.");
                var name = CliArgs.GetOption(args, "--name") ?? "Standard Template";
                var req = new CreateMeetingTemplateRequest
                {
                    MeetingId = meetingId,
                    Name = name,
                    SaveRecurrence = CliArgs.HasFlag(args, "--save-recurrence")
                };
                var created = await client.CreateMeetingTemplateAsync(userId, req);
                CliOutput.WriteSuccess($"Meeting template '{name}' created successfully.");
                CliOutput.WriteJson(created);
                break;

            default:
                Console.WriteLine("Usage: zoom-meetings templates <list|create> [options]");
                break;
        }
    }
}
