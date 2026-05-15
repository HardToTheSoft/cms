namespace Cms.Api.Dto;


public sealed class EntityDto
{
  public string Id { get; set; }

  public int Version { get; set; }

  public bool Published { get; set; }
  public bool Disabled { get; set; }

  public string? PayloadJson { get; set; }
}