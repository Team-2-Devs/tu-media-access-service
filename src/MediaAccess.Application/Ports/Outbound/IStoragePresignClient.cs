using MediaAccess.Application.Ports.Outbound.Contracts;

namespace MediaAccess.Application.Ports.Outbound;

public interface IStoragePresignClient
{
  public Task<StoragePresignGetResponse> PresignGetAsync(StoragePresignGetRequest req, CancellationToken ct = default);
}
