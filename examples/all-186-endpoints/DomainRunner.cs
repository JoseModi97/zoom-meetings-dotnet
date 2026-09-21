namespace ZoomMeetings.Examples.AllEndpoints;

public static class DomainRunner
{
    public static async Task RunOperationAsync(string operationId, string methodAndPath, Func<Task> action)
    {
        try
        {
            await action();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  [2xx] ");
            Console.ResetColor();
            Console.WriteLine($"{operationId} ({methodAndPath})");
        }
        catch (ZoomApiException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"  [{(int)ex.StatusCode}] ");
            Console.ResetColor();
            Console.WriteLine($"{operationId} ({methodAndPath}) -> Code {ex.ZoomCode}: {ex.ZoomMessage}");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("  [ERR] ");
            Console.ResetColor();
            Console.WriteLine($"{operationId} ({methodAndPath}) -> {ex.GetType().Name}: {ex.Message}");
        }
    }
}
