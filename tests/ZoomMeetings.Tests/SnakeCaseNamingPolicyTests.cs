using ZoomMeetings.Internal;
using Xunit;

namespace ZoomMeetings.Tests;

public class SnakeCaseNamingPolicyTests
{
    [Theory]
    [InlineData("NextPageToken", "next_page_token")]
    [InlineData("AccountId", "account_id")]
    [InlineData("HostId", "host_id")]
    [InlineData("Topic", "topic")]
    [InlineData("StartTime", "start_time")]
    [InlineData("TotalRecords", "total_records")]
    [InlineData("SipDialing", "sip_dialing")]
    [InlineData("Id", "id")]
    public void ConvertName_ProducesExpectedSnakeCase(string input, string expected)
    {
        var actual = SnakeCaseNamingPolicy.Instance.ConvertName(input);
        Assert.Equal(expected, actual);
    }
}
