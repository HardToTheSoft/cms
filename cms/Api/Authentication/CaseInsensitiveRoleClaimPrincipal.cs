using System.Security.Claims;


namespace Cms.Api.Authentication;


public class CaseInsensitiveRoleClaimPrincipal : ClaimsPrincipal
{
  #region Constructor
  public CaseInsensitiveRoleClaimPrincipal(ClaimsPrincipal principal)
    : base(principal)
  { }
  #endregion


  #region Public methods
  public override bool IsInRole(string role)
    => Claims.Any(c => c.Type == ClaimTypes.Role
      && string.Equals(c.Value, role, StringComparison.OrdinalIgnoreCase));
  #endregion
}