using MediaAccess.Application.Ports.Outbound.Contracts;

namespace MediaAccess.Application.Ports.Outbound;

/// <summary>Defines the outbound port for communicating with the Storage service to request presigned GET URLs.</summary>
public interface IStoragePresignClient
{
  /// <summary>
  /// Sends a request to the Storage service to generate a presigned GET URL for the specified object.
  /// </summary>
  /// <param name="req">The request containing the object key and TTL.</param>
  /// <param name="ct">Cancellation token.</param>
  /// <returns>The response containing the presigned URL and its expiry time.</returns>
  public Task<StoragePresignGetResponse> PresignGetAsync(StoragePresignGetRequest req, CancellationToken ct = default);
}
