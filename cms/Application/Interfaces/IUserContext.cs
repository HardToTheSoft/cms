namespace Cms.Application.Interfaces;


public interface IUserContext
{
  bool IsUser { get; }
  bool IsAdmin { get; }
}