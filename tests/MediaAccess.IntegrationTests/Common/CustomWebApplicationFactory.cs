using MediaAccess.Application.Ports.Outbound;
using MediaAccess.Application.Ports.Outbound.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace MediaAccess.IntegrationTests.Common;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.UseEnvironment("Test");

    builder.ConfigureServices(services =>
    {
      // Replace Storage HTTP client adapter with deterministic fake
      var descriptors = services
        .Where(d => d.ServiceType == typeof(IStoragePresignClient))
        .ToList();

      foreach (var d in descriptors)
        services.Remove(d);

      services.AddSingleton<IStoragePresignClient, FakeStoragePresignClient>();
    });
  }
}

// Integration-only fake
file sealed class FakeStoragePresignClient : IStoragePresignClient
{
  public string UrlToReturn { get; set; } = "https://storage.example.com/presigned";

  public DateTimeOffset ExpiresAtToReturn { get; set; } = DateTimeOffset.Parse("2030-01-01T00:00:00Z");

  public Task<StoragePresignGetResponse> PresignGetAsync(
    StoragePresignGetRequest req,
    CancellationToken ct = default)
  {
    return Task.FromResult(
      new StoragePresignGetResponse(UrlToReturn, ExpiresAtToReturn));
  }
}
