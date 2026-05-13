using System.Text;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;

using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;


namespace Cms.Infrastructure;


public sealed class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
  #region Members
  public const string SchemeName = "Basic";
  #endregion


  #region Constructor
  public BasicAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder urlEncoder)
      : base(options, logger, urlEncoder)
  { }
  #endregion


  #region Private methods
  protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
  {
    if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValue)
      || string.IsNullOrWhiteSpace(authorizationHeaderValue))
      return AuthenticateResult.Fail("Missing Authorization Header");

    try
    {
      var authenticationHeaderValue = AuthenticationHeaderValue.Parse(authorizationHeaderValue!);

      if (!authenticationHeaderValue.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
        return AuthenticateResult.Fail("Invalid Authorization Scheme");

      if (string.IsNullOrWhiteSpace(authenticationHeaderValue.Parameter))
        return AuthenticateResult.Fail("Invalid Authorization Header");

      var parameters = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationHeaderValue.Parameter))
        .Split(':', 2, StringSplitOptions.RemoveEmptyEntries);

      if (parameters.Length != 2)
        return AuthenticateResult.Fail("Invalid Authorization Header");

      var user = new User
      {
        UserName = parameters[0],
        Password = parameters[1]
      };

      if (!user.TryValidate(out var validationErrors))
        return AuthenticateResult.Fail("User validation failed.");

      List<Claim> claims = [
        new(ClaimTypes.NameIdentifier, user.UserName),
        new(ClaimTypes.Name, user.UserName)
      ];

      foreach (var role in user.Roles!)
        claims.Add(new(ClaimTypes.Role, role));

      var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));

      return AuthenticateResult.Success(new(principal, Scheme.Name));
    }
    catch
    {
      return AuthenticateResult.Fail("Invalid Authorization Header");
    }
  }
  #endregion
}