using System.Net.Http;
using ZoomMeetings.Internal;
using ZoomMeetings.Models;

namespace ZoomMeetings;

public sealed partial class ZoomClient
{
    /// <summary>GET /tsp - get account's TSP information.</summary>
    public Task<AccountTspSettings?> GetAccountTspAsync(CancellationToken cancellationToken = default)
        => CallAsync<AccountTspSettings>(HttpMethod.Get, "/tsp", cancellationToken: cancellationToken);

    /// <summary>PATCH /tsp - update account's TSP information.</summary>
    public Task UpdateAccountTspAsync(UpdateAccountTspSettingsRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, "/tsp", request, cancellationToken: cancellationToken);

    /// <summary>GET /users/{userId}/tsp - list user's TSP accounts.</summary>
    public Task<ListUserTspsResult?> ListUserTspsAsync(string userId, CancellationToken cancellationToken = default)
        => CallAsync<ListUserTspsResult>(HttpMethod.Get, $"/users/{ZoomIdEncoding.EncodePathSegment(userId)}/tsp", cancellationToken: cancellationToken);

    /// <summary>POST /users/{userId}/tsp - add a user's TSP account.</summary>
    public Task<UserTspAccount?> CreateUserTspAsync(string userId, CreateUserTspRequest request, CancellationToken cancellationToken = default)
        => CallAsync<UserTspAccount>(HttpMethod.Post, $"/users/{ZoomIdEncoding.EncodePathSegment(userId)}/tsp", request, cancellationToken: cancellationToken);

    /// <summary>PATCH /users/{userId}/tsp/settings - set global dial-in URL for a TSP user.</summary>
    public Task UpdateUserTspUrlAsync(string userId, string audioUrl, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/users/{ZoomIdEncoding.EncodePathSegment(userId)}/tsp/settings", new UpdateUserTspUrlRequest { AudioUrl = audioUrl }, cancellationToken: cancellationToken);

    /// <summary>GET /users/{userId}/tsp/{tspId} - get a user's TSP account.</summary>
    public Task<UserTspAccount?> GetUserTspAsync(string userId, string tspId, CancellationToken cancellationToken = default)
        => CallAsync<UserTspAccount>(HttpMethod.Get, $"/users/{ZoomIdEncoding.EncodePathSegment(userId)}/tsp/{Uri.EscapeDataString(tspId)}", cancellationToken: cancellationToken);

    /// <summary>PATCH /users/{userId}/tsp/{tspId} - update a TSP account.</summary>
    public Task UpdateUserTspAsync(string userId, string tspId, UpdateUserTspRequest request, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethods.Patch, $"/users/{ZoomIdEncoding.EncodePathSegment(userId)}/tsp/{Uri.EscapeDataString(tspId)}", request, cancellationToken: cancellationToken);

    /// <summary>DELETE /users/{userId}/tsp/{tspId} - delete a user's TSP account.</summary>
    public Task DeleteUserTspAsync(string userId, string tspId, CancellationToken cancellationToken = default)
        => CallAsync(HttpMethod.Delete, $"/users/{ZoomIdEncoding.EncodePathSegment(userId)}/tsp/{Uri.EscapeDataString(tspId)}", cancellationToken: cancellationToken);
}
