namespace MediaAccess.Application.Ports.Outbound.Contracts;

/// <summary>Response returned by the Storage service containing the presigned GET URL and its expiry time.</summary>
public sealed record StoragePresignGetResponse(string Url, DateTimeOffset ExpiresAt);
