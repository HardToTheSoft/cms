using System.Reflection;

using Cms.Application.Interfaces;


namespace Cms.Application.Extensions;


public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddEventHandlers(this IServiceCollection services, Assembly assembly)
  {
    var eventHandlerType = typeof(IEventHandlerService<>);

    var eventHandlers = assembly.GetTypes()
      .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == eventHandlerType)
        && !t.IsAbstract && !t.IsInterface);

    foreach (var eventHandler in eventHandlers)
    {
      var implementedInterface = eventHandler.GetInterfaces()
        .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == eventHandlerType);

      services.AddTransient(implementedInterface, eventHandler);
    }

    return services;
  }
}