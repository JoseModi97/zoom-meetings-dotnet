using ZoomMeetings.Cli;
using Xunit;

namespace ZoomMeetings.Tests;

public class CliArgsTests
{
    [Fact]
    public void GetOption_FindsValueAfterFlag()
    {
        var args = new[] { "--topic", "Sprint planning", "--duration", "30" };

        Assert.Equal("Sprint planning", CliArgs.GetOption(args, "--topic"));
        Assert.Equal("30", CliArgs.GetOption(args, "--duration"));
    }

    [Fact]
    public void GetOption_MissingFlag_ReturnsNull()
    {
        var args = new[] { "--topic", "Sprint planning" };

        Assert.Null(CliArgs.GetOption(args, "--agenda"));
    }

    [Fact]
    public void GetOption_FlagIsLastArgWithNoValue_ReturnsNull()
    {
        var args = new[] { "--topic", "Sprint planning", "--dry-run" };

        Assert.Null(CliArgs.GetOption(args, "--dry-run"));
    }

    [Fact]
    public void HasFlag_DetectsPresenceRegardlessOfPosition()
    {
        var args = new[] { "--topic", "x", "--yes" };

        Assert.True(CliArgs.HasFlag(args, "--yes"));
        Assert.False(CliArgs.HasFlag(args, "--no-such-flag"));
    }

    [Fact]
    public void ExtractProfile_NoProfileFlag_ReturnsArgsUnchanged()
    {
        var args = new[] { "meetings", "list", "--user-id", "me" };

        var (profile, remaining) = CliArgs.ExtractProfile(args);

        Assert.Null(profile);
        Assert.Equal(args, remaining);
    }

    [Fact]
    public void ExtractProfile_LongFlag_RemovesFlagAndValueFromRemainingArgs()
    {
        var args = new[] { "meetings", "list", "--profile", "Sandbox", "--user-id", "me" };

        var (profile, remaining) = CliArgs.ExtractProfile(args);

        Assert.Equal("Sandbox", profile);
        Assert.Equal(new[] { "meetings", "list", "--user-id", "me" }, remaining);
    }

    [Fact]
    public void ExtractProfile_ShortFlag_RemovesFlagAndValueFromRemainingArgs()
    {
        var args = new[] { "raw", "GET", "/meetings/123", "-p", "Sandbox" };

        var (profile, remaining) = CliArgs.ExtractProfile(args);

        Assert.Equal("Sandbox", profile);
        Assert.Equal(new[] { "raw", "GET", "/meetings/123" }, remaining);
    }
}
