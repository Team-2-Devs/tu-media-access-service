using MediaAccess.Application.Ports.Inbound;
using MediaAccess.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace MediaAccess.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    // Use cases
    services.AddScoped<IRequestImageAccess, RequestImageAccess>();
    
    return services;
  }
}
