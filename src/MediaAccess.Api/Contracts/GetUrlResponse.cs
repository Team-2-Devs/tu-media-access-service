namespace MediaAccess.Api.Contracts;

public sealed record GetUrlResponse(string Url, DateTimeOffset ExpiresAt);
