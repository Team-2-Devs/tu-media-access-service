namespace MediaAccess.Application.Ports.Outbound.Contracts;

public sealed record StoragePresignGetRequest(string ObjectKey, int TtlSec);
