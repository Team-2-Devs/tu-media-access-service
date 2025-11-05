namespace MediaAccess.Application.Ports.Inbound.Contracts;

public abstract record RequestImageAccessResult
{
  public sealed record Success(string Url, DateTimeOffset ExpiresAt, string ContentType) : RequestImageAccessResult;
  public sealed record NotFound(string ObjectKey) : RequestImageAccessResult;
  public sealed record Invalid(Dictionary<string, string[]> Errors) : RequestImageAccessResult;
}
