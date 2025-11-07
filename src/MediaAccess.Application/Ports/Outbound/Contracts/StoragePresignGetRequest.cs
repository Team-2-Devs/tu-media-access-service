namespace MediaAccess.Application.Ports.Outbound.Contracts;

/// <summary>Request sent to the Storage service for generating a presigned GET URL.</summary>
public sealed record StoragePresignGetRequest(string ObjectKey, int TtlSec);
