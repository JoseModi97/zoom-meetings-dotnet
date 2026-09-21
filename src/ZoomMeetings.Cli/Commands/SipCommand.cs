using ZoomMeetings.Models;

namespace ZoomMeetings.Cli.Commands;

internal static class SipCommand
{
    public static async Task ExecuteAsync(string[] args, string? profile)
    {
        var action = args.Length > 0 ? args[0].ToLowerInvariant() : "list";
        var config = ZoomCliConfigLoader.LoadConfig(profile);
        using var client = new ZoomClient(config);

        switch (action)
        {
            case "list":
                var phones = await client.ListSipPhonesAsync();
                CliOutput.WriteJson(phones);
                break;

            case "enable":
                var email = CliArgs.GetOption(args, "--email")
                    ?? throw new ArgumentException("Missing required --email option.");
                var domain = CliArgs.GetOption(args, "--domain")
                    ?? throw new ArgumentException("Missing required --domain option.");
                var authName = CliArgs.GetOption(args, "--auth-name") ?? email;
                var pass = CliArgs.GetOption(args, "--password") ?? "";
                var server = CliArgs.GetOption(args, "--server") ?? domain;

                var req = new EnableSipPhoneRequest
                {
                    UserEmail = email,
                    UserName = authName,
                    Domain = domain,
                    AuthorizationName = authName,
                    Password = pass,
                    Server = server
                };
                var enabled = await client.EnableSipPhoneAsync(req);
                CliOutput.WriteSuccess($"SIP Phone enabled.");
                CliOutput.WriteJson(enabled);
                break;

            case "delete":
                var phoneId = CliArgs.GetOption(args, "--phone-id")
                    ?? throw new ArgumentException("Missing required --phone-id option.");
                await client.DeleteSipPhoneAsync(phoneId);
                CliOutput.WriteSuccess($"SIP Phone '{phoneId}' deleted.");
                break;

            default:
                Console.WriteLine("Usage: zoom-meetings sip <list|enable|delete> [options]");
                break;
        }
    }
}
