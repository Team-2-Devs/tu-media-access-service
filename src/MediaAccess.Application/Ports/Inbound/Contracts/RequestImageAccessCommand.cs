namespace MediaAccess.Application.Ports.Inbound.Contracts;

/// <summary>Command data for requesting a presigned GET URL for an object.</summary>
public sealed record RequestImageAccessCommand(string ObjectKey);
