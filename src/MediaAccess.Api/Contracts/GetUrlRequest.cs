namespace MediaAccess.Api.Contracts;

/// <summary>Request for retrieving a presigned GET URL for an existing object.</summary>
public sealed record GetUrlRequest(string ObjectKey);
