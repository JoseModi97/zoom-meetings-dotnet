namespace ZoomMeetings.Cli.Commands;

internal static class TspCommand
{
    public static async Task ExecuteAsync(string[] args, string? profile)
    {
        var action = args.Length > 0 ? args[0].ToLowerInvariant() : "get-account";
        var config = ZoomCliConfigLoader.LoadConfig(profile);
        using var client = new ZoomClient(config);

        switch (action)
        {
            case "get-account":
            case "account":
                var accountTsp = await client.GetAccountTspAsync();
                CliOutput.WriteJson(accountTsp);
                break;

            case "get-user":
            case "user":
                var userId = CliArgs.GetOption(args, "--user-id") ?? "me";
                var userTsps = await client.ListUserTspsAsync(userId);
                CliOutput.WriteJson(userTsps);
                break;

            default:
                Console.WriteLine("Usage: zoom-meetings tsp <get-account|get-user> [--user-id <id>]");
                break;
        }
    }
}
