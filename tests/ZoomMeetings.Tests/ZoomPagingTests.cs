using Xunit;

namespace ZoomMeetings.Tests;

public class ZoomPagingTests
{
    private record Page(List<string> Items, string? NextPageToken);

    [Fact]
    public async Task EnumerateAsync_WalksAllPagesUntilNextPageTokenIsEmpty()
    {
        var pages = new Dictionary<string, Page>
        {
            [""] = new(new List<string> { "a", "b" }, "page2"),
            ["page2"] = new(new List<string> { "c" }, "page3"),
            ["page3"] = new(new List<string> { "d", "e" }, null!),
        };

        var callCount = 0;

        Task<Page> FetchPage(string? token, CancellationToken ct)
        {
            callCount++;
            return Task.FromResult(pages[token ?? ""]);
        }

        var items = new List<string>();
        await foreach (var item in ZoomPaging.EnumerateAsync<Page, string>(FetchPage, p => p.Items, p => p.NextPageToken))
        {
            items.Add(item);
        }

        Assert.Equal(new[] { "a", "b", "c", "d", "e" }, items);
        Assert.Equal(3, callCount);
    }

    [Fact]
    public async Task EnumerateAsync_SinglePage_YieldsItsItemsOnly()
    {
        Task<Page> FetchPage(string? token, CancellationToken ct) => Task.FromResult(new Page(new List<string> { "only" }, null));

        var items = new List<string>();
        await foreach (var item in ZoomPaging.EnumerateAsync<Page, string>(FetchPage, p => p.Items, p => p.NextPageToken))
        {
            items.Add(item);
        }

        Assert.Equal(new[] { "only" }, items);
    }
}
