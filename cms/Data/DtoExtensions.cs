namespace Cms.Data;


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
      PayloadJson = entity.PayloadJson
    };
  }
}