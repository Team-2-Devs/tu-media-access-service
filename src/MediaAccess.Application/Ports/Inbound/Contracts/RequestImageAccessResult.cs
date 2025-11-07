namespace MediaAccess.Application.Ports.Inbound.Contracts;

/// <summary>Result of a media access request for a presigned GET URL.</summary>
public abstract record RequestImageAccessResult
{
  /// <summary>Returned when validation fails.</summary>
  public sealed record Invalid(Dictionary<string, string[]> Errors) : RequestImageAccessResult;

  /// <summary>Returned when the requested object key was not found.</summary>
  public sealed record NotFound(string ObjectKey) : RequestImageAccessResult;

  /// <summary>Returned when the presigned URL was successfully generated.</summary>
  public sealed record Success(string Url, DateTimeOffset ExpiresAt) : RequestImageAccessResult;
}
