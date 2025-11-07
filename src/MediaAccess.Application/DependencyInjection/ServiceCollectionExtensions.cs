using MediaAccess.Application.Ports.Inbound;
using MediaAccess.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace MediaAccess.Application.DependencyInjection;

/// <summary>Dependency injection extensions for registering Application-layer services.</summary>
public static class ServiceCollectionExtensions
{
  /// <summary>Registers all Application-layer services, including use cases.</summary>
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    // Use cases
    services.AddScoped<IRequestImageAccess, RequestImageAccess>();
    
    return services;
  }
}
