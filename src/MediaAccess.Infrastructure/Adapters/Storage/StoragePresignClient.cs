using System.Net.Http.Json;
using MediaAccess.Application.Ports.Outbound;
using MediaAccess.Application.Ports.Outbound.Contracts;

namespace MediaAccess.Infrastructure.Adapters.Storage;

/// <summary>HTTP client adapter for communicating with the Storage service to request presigned GET URLs.</summary>
public sealed class StoragePresignClient(HttpClient http) : IStoragePresignClient
{
  public async Task<StoragePresignGetResponse> PresignGetAsync(StoragePresignGetRequest req, CancellationToken ct = default)
  {
    var response = await http.PostAsJsonAsync("/internal/v1/storage/presign-get", req, ct);

    response.EnsureSuccessStatusCode();

    var body = await response.Content.ReadFromJsonAsync<StoragePresignGetResponse>(cancellationToken: ct);

    return body 
      ?? throw new InvalidOperationException("Empty response from Storage Service");
  }
}
