namespace ZoomMeetings.Cli;

internal static class CliArgs
{
    public static string? GetOption(string[] args, string name)
    {
        for (var i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == name) return args[i + 1];
        }
        return null;
    }

    public static bool HasFlag(string[] args, string name) => Array.IndexOf(args, name) >= 0;

    /// <summary>Extracts and removes --profile/-p from args, leaving the rest untouched for further parsing.</summary>
    public static (string? Profile, string[] RemainingArgs) ExtractProfile(string[] args)
    {
        var profile = GetOption(args, "--profile") ?? GetOption(args, "-p");
        if (profile == null) return (null, args);

        var rest = new List<string>(args.Length);
        for (var i = 0; i < args.Length; i++)
        {
            if ((args[i] == "--profile" || args[i] == "-p") && i + 1 < args.Length) { i++; continue; }
            rest.Add(args[i]);
        }
        return (profile, rest.ToArray());
    }
}
