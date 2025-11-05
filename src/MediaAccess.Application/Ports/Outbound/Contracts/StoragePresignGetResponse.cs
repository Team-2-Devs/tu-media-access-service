namespace MediaAccess.Application.Ports.Outbound.Contracts;

public sealed record StoragePresignGetResponse(string Url, DateTimeOffset ExpiresAt);
