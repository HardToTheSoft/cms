using System.ComponentModel.DataAnnotations;


namespace Cms.Infrastructure.Attributes;


public sealed class GuidValidationAttribute : ValidationAttribute
{
  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    if (!Guid.TryParse(value?.ToString(), out var guid) || guid == Guid.Empty)
      return new ValidationResult(ErrorMessage);

    return ValidationResult.Success;
  }
}