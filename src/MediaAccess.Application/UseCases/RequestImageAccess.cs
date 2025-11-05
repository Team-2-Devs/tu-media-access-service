using MediaAccess.Application.Ports.Inbound;
using MediaAccess.Application.Ports.Inbound.Contracts;
using MediaAccess.Application.Ports.Outbound;
using MediaAccess.Application.Ports.Outbound.Contracts;

namespace MediaAccess.Application.UseCases;

public sealed class RequestImageAccess : IRequestImageAccess
{
  private const int DefaultTtlSec = 300; // 5 minutes – presigned URL lifetime

  private readonly IStoragePresignClient _storage;

  public RequestImageAccess(IStoragePresignClient storage) => _storage = storage;

  public async Task<RequestImageAccessResult> ExecuteAsync(RequestImageAccessCommand cmd, CancellationToken ct = default)
  {
    if (string.IsNullOrWhiteSpace(cmd.ObjectKey))
    {
      return new RequestImageAccessResult.Invalid(new() 
      {
        { "objectKey", new[] { "Required" } },
      });
    }

    var result = await _storage.PresignGetAsync(
      new StoragePresignGetRequest(cmd.ObjectKey, DefaultTtlSec), ct);

    return new RequestImageAccessResult.Success(result.Url, result.ExpiresAt);
  }
}
