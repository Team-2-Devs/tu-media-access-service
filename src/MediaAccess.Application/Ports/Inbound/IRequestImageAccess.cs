using MediaAccess.Application.Ports.Inbound.Contracts;

namespace MediaAccess.Application.Ports.Inbound;

/// <summary>Defines the inbound port for requesting a presigned GET URL for media.</summary>
public interface IRequestImageAccess
{
  /// <summary>
  /// Retrieves a presigned GET URL for the specified object key.
  /// </summary>
  /// <param name="cmd">The request parameters containing the object key.</param>
  /// <param name="ct">Cancellation token.</param>
  /// <returns>A result indicating success, validation failure, or that the object was not found.</returns>
  public Task<RequestImageAccessResult> ExecuteAsync(RequestImageAccessCommand cmd, CancellationToken ct = default);
}
