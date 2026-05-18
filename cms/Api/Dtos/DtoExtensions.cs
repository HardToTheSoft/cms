using System.Text.Json;

using Cms.Api.Dto;
using Cms.Infrastructure.Entities;


namespace Cms.Domain.Extensions;


public static class DtoExtensions
{
  public static UserEntityDto? ToUserEntityDto(this Entity? entity)
  {
    if (entity is null)
      return null;

    return new UserEntityDto
    {
      Id = entity.Id,
      Version = entity.Version,
      Published = entity.Published,
      PayloadJson = entity.PayloadJson ?? JsonDocument.Parse("{}"),
      CreatedAt = entity.UpdatedAt
    };
  }


  public static AdminEntityDto? ToAdminEntityDto(this Entity? entity)
  {
    if (entity is null)
      return null;

    return new AdminEntityDto
    {
      Id = entity.Id,
      Version = entity.Version,
      Published = entity.Published,
      Disabled = entity.Disabled,
      PayloadJson = entity.PayloadJson ?? JsonDocument.Parse("{}"),
      CreatedAt = entity.UpdatedAt
    };
  }
}