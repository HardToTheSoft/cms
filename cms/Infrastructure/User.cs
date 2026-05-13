using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;


namespace Cms.Infrastructure;


public sealed class User
{
  #region Properties
  [Display(Name = "User Name")]
  [Required(ErrorMessage = "{0} is required.")]
  [RegularExpression(@"^\S+$", ErrorMessage = "{0} cannot contain spaces.")]
  [StringLength(20, MinimumLength = 10, ErrorMessage = "{0} must be between 10 and 20 characters long.")]
  public string? UserName { get; init; }


  [Required(ErrorMessage = "{0} is required.")]
  [GuidValidation(ErrorMessage = "{0} must be a valid non-empty Guid.")]
  public string? Password { get; init; }


  [MinLength(1, ErrorMessage = "{0} must have at least one role assigned.")]
  public string[]? Roles { get; private set; }
  #endregion


  #region Public methods
  public bool TryValidate(out List<ValidationResult> errors)
  {
    errors = [];

    bool isValid = Validator.TryValidateObject(this, new(this), errors, true);

    if (isValid)
      CalculateRoles();

    return isValid;
  }
  #endregion


  #region Private methods
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private void CalculateRoles()
  {
    Roles = UserName! switch
    {
      var s when s.StartsWith("admin_", StringComparison.OrdinalIgnoreCase) => ["Admin", "User"],
      var s when s.StartsWith("user_", StringComparison.OrdinalIgnoreCase) => ["User"],
      var s when s.StartsWith("cms_", StringComparison.OrdinalIgnoreCase) => ["CMS"],
      _ => []
    };
  }
  #endregion
}