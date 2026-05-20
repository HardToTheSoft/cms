
using System.Reflection;

using Cms.Application.Interfaces;
using Cms.Infrastructure.Attributes;


namespace Cms.Infrastructure.EventHandler;


public abstract class EventHandlerServiceAbstract<T> : IEventHandlerService<T>
   where T : class, IEvent, IEventHandler<T>, new()
{
  #region Public methods
  public abstract Task HandleEventAsync(T eventToHandle);
  #endregion


  #region Private methods
  protected async Task InitAsync(IEvent eventModel, params object?[]? args)
  {
    ArgumentNullException.ThrowIfNull(eventModel);

    //...
  }


  protected void InitMethods(ref Dictionary<string, MethodInfo>? methods)
  {
    if (methods is not null)
      return;

    methods = [];

    TopicsAttribute? topics;

    foreach (var methodInfo in GetType()
      .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
      .Where(mi => Attribute.IsDefined(mi, typeof(TopicsAttribute), false)
        && (mi.ReturnType == typeof(void) || mi.ReturnType == typeof(Task))))
    {
      topics = methodInfo.GetCustomAttribute<TopicsAttribute>();

      foreach (var topic in topics!.Topics)
        methods.Add(topic, methodInfo);
    }
  }


  protected async Task DispatchEventAsync(Dictionary<string, MethodInfo>? methods, IEvent eventToProcess)
  {
    ArgumentNullException.ThrowIfNull(methods);

    var methodInfo = methods.FirstOrDefault(kvp => kvp.Key.Equals(eventToProcess.Topic)).Value;

    ArgumentNullException.ThrowIfNull(methodInfo);

    if (methodInfo.ReturnType == typeof(Task))
    {
      Task task = (Task)methodInfo.Invoke(this, [eventToProcess])!;

      await task;
    }
    else
      methodInfo.Invoke(this, [eventToProcess]);
  }
  #endregion
}