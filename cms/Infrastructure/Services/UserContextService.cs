using Cms.Application.Interfaces;


namespace Cms.Infrastructure.Services;


public class UserContextService : IUserContext
{
  #region Constructor
  public UserContextService(IHttpContextAccessor httpContextAccessor)
  {
    IsUser = httpContextAccessor.HttpContext?.User.IsInRole("User") ?? false;
    IsAdmin = httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;
  }
  #endregion


  #region Properties
  public bool IsUser { get; }
  public bool IsAdmin { get; }
  #endregion
}