using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>
    /// Calls a Zoom Meetings API operation that expects a multipart/form-data file upload (e.g. webinar
    /// branding virtual backgrounds/wallpaper). Reaches any file-upload endpoint, typed wrapper or not.
    /// </summary>
    public async Task<TResponse?> UploadFileAsync<TResponse>(
        HttpMethod method,
        string path,
        Stream fileStream,
        string fileName,
        string fieldName = "file",
        IDictionary<string, string>? additionalFields = null,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendFileAsync(method, path, fileStream, fileName, fieldName, additionalFields, cancellationToken).ConfigureAwait(false);
        return await DeserializeAsync<TResponse>(response, cancellationToken).ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage> SendFileAsync(
        HttpMethod method,
        string path,
        Stream fileStream,
        string fileName,
        string fieldName,
        IDictionary<string, string>? additionalFields,
        CancellationToken cancellationToken)
    {
        var requestUri = BuildRequestUri(path, null);
        using var request = new HttpRequestMessage(method, requestUri);

        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), fieldName, fileName);
        if (additionalFields != null)
        {
            foreach (var field in additionalFields)
                content.Add(new StringContent(field.Value), field.Key);
        }
        request.Content = content;

        _logger?.LogDebug("Zoom API file upload: {Method} {Uri}", method, requestUri);

        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            try
            {
                await ThrowZoomApiExceptionAsync(response, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                response.Dispose();
            }
        }

        return response;
    }
}
