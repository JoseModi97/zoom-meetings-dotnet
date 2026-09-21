using System.Runtime.CompilerServices;

namespace ZoomMeetings;

/// <summary>
/// Walks Zoom's next_page_token-based list endpoints as a single async stream. Every typed list
/// method has its own result type (the array field is named differently per endpoint - "meetings",
/// "registrants", "polls", etc.), so this stays generic over both the page/result type and the item
/// type rather than assuming one shared "PagedResult&lt;T&gt;" shape.
/// </summary>
public static class ZoomPaging
{
    /// <summary>
    /// Repeatedly calls <paramref name="fetchPage"/> (passing the previous page's next_page_token,
    /// starting with null) and yields the items from each page via <paramref name="selectItems"/>,
    /// stopping once <paramref name="selectNextToken"/> returns null/empty.
    /// </summary>
    public static async IAsyncEnumerable<TItem> EnumerateAsync<TResult, TItem>(
        Func<string?, CancellationToken, Task<TResult>> fetchPage,
        Func<TResult, IEnumerable<TItem>?> selectItems,
        Func<TResult, string?> selectNextToken,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string? nextPageToken = null;

        do
        {
            var page = await fetchPage(nextPageToken, cancellationToken).ConfigureAwait(false);

            var items = selectItems(page);
            if (items != null)
            {
                foreach (var item in items)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    yield return item;
                }
            }

            nextPageToken = selectNextToken(page);
        }
        while (!string.IsNullOrEmpty(nextPageToken));
    }
}
