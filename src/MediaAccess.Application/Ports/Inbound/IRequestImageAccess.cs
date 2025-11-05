using MediaAccess.Application.Ports.Inbound.Contracts;

namespace MediaAccess.Application.Ports.Inbound;

public interface IRequestImageAccess
{
  Task<RequestImageAccessResult> ExecuteAsync(RequestImageAccessCommand cmd, CancellationToken ct = default);
}
