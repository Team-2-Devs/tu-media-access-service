using MediaAccess.Application.Ports.Outbound;
using MediaAccess.Application.Ports.Outbound.Contracts;

namespace MediaAccess.UnitTests.Common.Doubles;

public sealed class FakeStoragePresignClient : IStoragePresignClient
{
  public StoragePresignGetRequest? LastGetRequest { get; private set; }

  public string UrlToReturn { get; set; } = "http://local/upload";

  public DateTimeOffset ExpiresAtToReturn { get; set; } =
    DateTimeOffset.Parse("2030-01-01T00:00:00Z");

  public Task<StoragePresignGetResponse> PresignGetAsync(
    StoragePresignGetRequest req,
    CancellationToken ct = default)
  {
    LastGetRequest = req;

    return Task.FromResult(
      new StoragePresignGetResponse(UrlToReturn, ExpiresAtToReturn));
  }
}
