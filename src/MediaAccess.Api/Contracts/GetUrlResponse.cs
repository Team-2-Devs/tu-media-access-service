namespace MediaAccess.Api.Contracts;

/// <summary>Response containing a presigned GET URL and its expiry time.</summary>
public sealed record GetUrlResponse(string Url, DateTimeOffset ExpiresAt);
