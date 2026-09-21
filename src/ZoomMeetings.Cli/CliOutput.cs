using System.Text.Json;

namespace ZoomMeetings.Cli;

internal static class CliOutput
{
    private static readonly JsonSerializerOptions PrettyOptions = new() { WriteIndented = true };

    public static void WriteJson<T>(T value)
    {
        Console.WriteLine(JsonSerializer.Serialize(value, PrettyOptions));
    }

    public static void WriteSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
