using Cms.Api.Dto;
using Cms.Infrastructure.Entities;


namespace Cms.Domain.Extensions;


public static class DtoExtensions
{
  public static EntityDto? ToDto(this Entity? entity)
  {
    if (entity is null)
      return null;

    return new EntityDto
    {
      Id = entity.Id,
      Version = entity.Version,
      Published = entity.Published,
      Disabled = entity.Disabled,
      PayloadJson = entity.Payload,
      CreatedAt = entity.UpdatedAt
    };
  }
}