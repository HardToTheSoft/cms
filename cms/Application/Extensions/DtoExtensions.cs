using Cms.Data;
using Cms.Api.Dto;


namespace Cms.Domain.Extensions;


public static class DtoExtensions
{
  public static EntityDto? ToDto(this EntityEntity? entity)
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