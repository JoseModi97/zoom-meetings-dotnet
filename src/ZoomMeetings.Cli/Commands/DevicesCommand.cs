using ZoomMeetings.Models;

namespace ZoomMeetings.Cli.Commands;

internal static class DevicesCommand
{
    public static async Task ExecuteAsync(string[] args, string? profile)
    {
        var action = args.Length > 0 ? args[0].ToLowerInvariant() : "list";
        var config = ZoomCliConfigLoader.LoadConfig(profile);
        using var client = new ZoomClient(config);

        switch (action)
        {
            case "list":
                var devices = await client.ListDevicesAsync();
                CliOutput.WriteJson(devices);
                break;

            case "get":
                var deviceId = CliArgs.GetOption(args, "--device-id")
                    ?? throw new ArgumentException("Missing required --device-id option.");
                var dev = await client.GetDeviceAsync(deviceId);
                CliOutput.WriteJson(dev);
                break;

            case "groups":
                var groups = await client.ListDeviceGroupsAsync();
                CliOutput.WriteJson(groups);
                break;

            case "h323-list":
            case "h323":
                var h323 = await client.ListH323DevicesAsync();
                CliOutput.WriteJson(h323);
                break;

            case "h323-create":
                var name = CliArgs.GetOption(args, "--name")
                    ?? throw new ArgumentException("Missing required --name option.");
                var ip = CliArgs.GetOption(args, "--ip")
                    ?? throw new ArgumentException("Missing required --ip option.");
                var devType = CliArgs.GetOption(args, "--type") ?? "H.323";
                var created = await client.CreateH323DeviceAsync(new CreateH323DeviceRequest
                {
                    Name = name,
                    Ip = ip,
                    Protocol = devType,
                    Encryption = "auto"
                });
                CliOutput.WriteSuccess($"H.323 device '{name}' registered.");
                CliOutput.WriteJson(created);
                break;

            default:
                Console.WriteLine("Usage: zoom-meetings devices <list|get|groups|h323-list|h323-create> [options]");
                break;
        }
    }
}
