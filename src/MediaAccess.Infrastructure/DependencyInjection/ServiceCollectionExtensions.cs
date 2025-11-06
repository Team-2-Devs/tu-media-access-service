using MediaAccess.Application.Ports.Outbound;
using MediaAccess.Infrastructure.Adapters.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediaAccess.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
  {
    AddStorage(services, config);

    return services;
  }

  private static void AddStorage(IServiceCollection services, IConfiguration config)
  {
    services.AddHttpClient<IStoragePresignClient, StoragePresignClient>(client =>
    {
      var baseUrl = config["Storage:BaseUrl"] ?? throw new InvalidOperationException("Storage:BaseUrl not configured");

      client.BaseAddress = new Uri(baseUrl);

      var token = config["Storage:InternalAccess"];
      if (!string.IsNullOrWhiteSpace(token))
        client.DefaultRequestHeaders.Add("X-Internal-Token", token);
    });
  }
}
